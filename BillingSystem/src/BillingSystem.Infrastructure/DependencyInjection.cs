using BillingSystem.Application.Abstractions.ExternalBillingService;
using BillingSystem.Infrastructure.Data;
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
            AddExternalServices(services, configuration);

            AddDbContext(services, configuration);

            return services;
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
