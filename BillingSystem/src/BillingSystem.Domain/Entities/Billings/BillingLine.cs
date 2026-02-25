using BillingSystem.Domain.Entities.Base;
using BillingSystem.Domain.Entities.Products;

namespace BillingSystem.Domain.Entities.Billings
{
    public class BillingLine : BaseEntity
    {
        public Guid BillingId { get; private set; }
        public Guid ProductId { get; private set; }
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        protected BillingLine() { }

        public BillingLine(Guid id, Guid billingId, Guid productId, int quantity, decimal unitPrice)
        {
            Id = id;
            BillingId = billingId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public decimal GetSubtotal()
        {
            return Quantity * UnitPrice;
        }
    }
}
