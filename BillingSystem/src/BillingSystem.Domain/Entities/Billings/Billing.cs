using BillingSystem.Domain.Entities.Base;
using BillingSystem.Domain.Entities.Customers;

namespace BillingSystem.Domain.Entities.Billings
{
    public class Billing : BaseEntity
    {
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; }

        private readonly List<BillingLine> _lines = new();
        public IReadOnlyCollection<BillingLine> Lines => _lines.AsReadOnly();

        protected Billing() { }

        public Billing(Guid id, Guid customerId)
        {
            Id = id;
            CustomerId = customerId;
        }

        public void AddLine(Guid productId, int quantity, decimal unitPrice)
        {
            var line = new BillingLine(Guid.NewGuid(), this.Id, productId, quantity, unitPrice);
            _lines.Add(line);
        }

        public decimal GetTotalAmount()
        {
            return _lines.Sum(l => l.GetSubtotal());
        }
    }
}
