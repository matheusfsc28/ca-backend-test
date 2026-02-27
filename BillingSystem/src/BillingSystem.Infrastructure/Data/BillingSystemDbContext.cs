using BillingSystem.Domain.Entities.Base;
using BillingSystem.Domain.Entities.Billings;
using BillingSystem.Domain.Entities.Customers;
using BillingSystem.Domain.Entities.Products;
using BillingSystem.Infrastructure.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace BillingSystem.Infrastructure.Data
{
    public class BillingSystemDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<BillingLine> BillingLines { get; set; }

        public BillingSystemDbContext(DbContextOptions<BillingSystemDbContext> options) : base(options) { }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder
                .Properties<DateTime>()
                .HaveConversion<UtcDateTimeConverter>();

            configurationBuilder
                .Properties<DateTime?>()
                .HaveConversion<UtcDateTimeConverter>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BillingSystemDbContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => e.ClrType != null && typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var propertyAccess = Expression.Property(parameter, nameof(BaseEntity.DeletedAt));
                var nullValue = Expression.Constant(null, typeof(DateTime?));
                var expression = Expression.Equal(propertyAccess, nullValue);
                var lambda = Expression.Lambda(expression, parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.Touch();
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
