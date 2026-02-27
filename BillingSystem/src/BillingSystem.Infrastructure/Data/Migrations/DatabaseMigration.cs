using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BillingSystem.Infrastructure.Data.Migrations
{
    public static class DatabaseMigration
    {
        public static async Task MigrateAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<BillingSystemDbContext>();

            await dbContext.Database.MigrateAsync();
        }
    }
}
