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

            builder.Property(b => b.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(200); ;

            builder.HasIndex(b => b.InvoiceNumber)
                .IsUnique();

            builder.Property(b => b.TotalAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(b => b.Currency)
                .IsRequired()
                .HasMaxLength(3);

            builder.HasMany(b => b.Lines)
                   .WithOne()
                   .HasForeignKey(bl => bl.BillingId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Customer)
                       .WithMany()
                       .HasForeignKey(b => b.CustomerId)
                       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
