using BillingSystem.Application.Abstractions.ExternalBillingService;
using BillingSystem.Domain.Interfaces.Data;
using BillingSystem.Domain.Interfaces.Repositories.Base;
using BillingSystem.Domain.Interfaces.Repositories.Billings;
using BillingSystem.Domain.Interfaces.Repositories.Customers;
using BillingSystem.Domain.Interfaces.Repositories.Products;
using BillingSystem.Infrastructure.Data;
using BillingSystem.Infrastructure.Repositories.Base;
using BillingSystem.Infrastructure.Repositories.Billings;
using BillingSystem.Infrastructure.Repositories.Customers;
using BillingSystem.Infrastructure.Repositories.Products;
using BillingSystem.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BillingSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services);
            AddExternalServices(services, configuration);
            AddUnitOfWork(services);

            AddDbContext(services, configuration);

            return services;
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseReadRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IBaseWriteRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IBillingReadRepository, BillingRepository>();
            services.AddScoped<IBillingWriteRepository, BillingRepository>();

            services.AddScoped<IBillingLineReadRepository, BillingLineRepository>();
            services.AddScoped<IBillingLineWriteRepository, BillingLineRepository>();

            services.AddScoped<ICustomerReadRepository, CustomerRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerRepository>();

            services.AddScoped<IProductReadRepository, ProductRepository>();
            services.AddScoped<IProductWriteRepository, ProductRepository>();
        }

        private static void AddUnitOfWork(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void AddExternalServices(IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue<string>("ExternalBillingServiceSettings:BaseUrl");

            services.AddHttpClient<IExternalBillingService, ExternalBillingService>(client =>
            {
                client.BaseAddress = new Uri(baseUrl!);
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddStandardResilienceHandler();
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("ConnectionStrings:PostgresqlConnection");

            services.AddDbContext<BillingSystemDbContext>(opt =>
            {
                opt.UseNpgsql(connectionString);
                opt.UseSnakeCaseNamingConvention();
            });
        }
    }
}
