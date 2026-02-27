using BillingSystem.Application.Abstractions.ExternalBillingService;
using BillingSystem.Domain.Entities.Customers;
using BillingSystem.Domain.Entities.Products;
using BillingSystem.Infrastructure.Data;
using CommonTestUtilities.Abstractions.ExternalBillingService;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private IList<Customer> _customers = new List<Customer>();
        private IList<Product> _products = new List<Product>();

        private readonly string _databaseName = $"InMemoryDbForTesting-{Guid.NewGuid()}";
        private bool _databaseInitialized = false;

        private readonly ServiceProvider _inMemoryProvider;

        public CustomWebApplicationFactory()
        {
            _inMemoryProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BillingSystemDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                var externalServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IExternalBillingService));
                if (externalServiceDescriptor != null)
                    services.Remove(externalServiceDescriptor);

                var mockExternalBillingService = new ExternalBillingServiceBuilder()
                    .ReturnsBillings()
                    .Build();

                services.AddScoped(provider => mockExternalBillingService);

                services.AddDbContext<BillingSystemDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_databaseName);
                    options.UseInternalServiceProvider(_inMemoryProvider);
                });

                using var scope = services.BuildServiceProvider().CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<BillingSystemDbContext>();

                if (!_databaseInitialized)
                {
                    dbContext.Database.EnsureDeleted();
                    dbContext.Database.EnsureCreated();

                    StartDatabase(dbContext);

                    _databaseInitialized = true;
                }
            });
        }

        public IEnumerable<Guid> GetCustomersIds() => _customers.Select(c => c.Id);
        public IEnumerable<Guid> GetProductsIds() => _products.Select(p => p.Id);

        private void StartDatabase(BillingSystemDbContext dbContext)
        {
            _customers.Clear();
            _products.Clear();

            for (int i = 0; i < 2; i++)
            {
                _customers.Add(CustomerBuilder.Build());
                _products.Add(ProductBuilder.Build());
            }

            dbContext.Customers.AddRange(_customers);
            dbContext.Products.AddRange(_products);

            dbContext.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _inMemoryProvider?.Dispose();
            }
        }
    }
}