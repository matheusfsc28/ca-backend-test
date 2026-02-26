using BillingSystem.Domain.Entities.Base;
using BillingSystem.Domain.Entities.Customers;

namespace BillingSystem.Domain.Entities.Billings
{
    public class Billing : BaseEntity
    {
        public string InvoiceNumber { get; private set; } = string.Empty;
        public Guid CustomerId { get; private set; }
        public DateTime Date { get; private set; }
        public DateTime DueDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        public string Currency { get; private set; } = string.Empty;
        private readonly List<BillingLine> _lines = new();
        public IReadOnlyCollection<BillingLine> Lines => _lines.AsReadOnly();
        public Customer Customer { get; private set; }

        protected Billing() { }

        public Billing(string invoiceNumber, Guid customerId, DateTime date, DateTime dueDate, decimal totalAmount, string currency)
        {
            Id = id;
            CustomerId = customerId;
            Date = date;
            DueDate = dueDate;
            TotalAmount = totalAmount;
            Currency = currency;
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
