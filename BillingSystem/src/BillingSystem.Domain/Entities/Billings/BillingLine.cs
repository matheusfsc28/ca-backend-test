using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
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
        public decimal Subtotal { get; private set; }

        protected BillingLine() { }

        public BillingLine(Guid billingId, Guid productId, int quantity, decimal unitPrice)
        {
            BillingId = billingId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Subtotal = GetSubtotal();

            Validate();
        }

        public decimal GetSubtotal()
        {
            return Quantity * UnitPrice;
        }

        private void Validate()
        {
            var errors = new List<string>();

            if (BillingId == Guid.Empty)
                errors.Add(ResourceMessagesException.BILLING_ID_EMPTY);

            if (ProductId == Guid.Empty)
                errors.Add(ResourceMessagesException.PRODUCT_ID_EMPTY);

            if (Quantity <= 0)
                errors.Add(ResourceMessagesException.QUANTITY_GREATER_THAN_ZERO);

            if (UnitPrice <= 0)
                errors.Add(ResourceMessagesException.UNIT_PRICE_GREATER_THAN_ZERO);

            if (errors.Count > 0)
                throw new ErrorOnValidationException(errors);
        }
    }
}
