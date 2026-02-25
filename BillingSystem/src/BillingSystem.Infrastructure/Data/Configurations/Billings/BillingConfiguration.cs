using BillingSystem.Domain.Entities.Billings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingSystem.Infrastructure.Data.Configurations.Billings
{
    public class BillingConfiguration : IEntityTypeConfiguration<Billing>
    {
        public void Configure(EntityTypeBuilder<Billing> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasMany(b => b.Lines)
                   .WithOne()
                   .HasForeignKey("BillingId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Customer)
                       .WithMany()
                       .HasForeignKey(b => b.CustomerId)
                       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
