using BillingSystem.Domain.Entities.Billings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillingSystem.Infrastructure.Data.Configurations.Billings
{
    public class BillingLineConfiguration : IEntityTypeConfiguration<BillingLine>
    {
        public void Configure(EntityTypeBuilder<BillingLine> builder)
        {
            builder.HasKey(bl => bl.Id);

            builder.Property(bl => bl.Quantity)
                   .IsRequired();

            builder.Property(bl => bl.UnitPrice)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.HasOne(bl => bl.Product)
                   .WithMany()
                   .HasForeignKey(bl => bl.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
