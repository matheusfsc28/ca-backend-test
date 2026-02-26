using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
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
            InvoiceNumber = invoiceNumber;
            CustomerId = customerId;
            Date = date;
            DueDate = dueDate;
            TotalAmount = totalAmount;
            Currency = currency;

            Validate();
        }

        private void Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(InvoiceNumber))
                errors.Add(ResourceMessagesException.INVOICE_NUMBER_EMPTY);

            if (CustomerId == Guid.Empty)
                errors.Add(ResourceMessagesException.CUSTOMER_ID_EMPTY);

            if (Date == default)
                errors.Add(ResourceMessagesException.DATE_EMPTY);

            if (DueDate == default)
                errors.Add(ResourceMessagesException.DUE_DATE_EMPTY);

            if (TotalAmount <= 0)
                errors.Add(ResourceMessagesException.TOTAL_AMOUNT_GREATER_THAN_ZERO);

            if (string.IsNullOrEmpty(Currency))
                errors.Add(ResourceMessagesException.CURRENCY_EMPTY);

            if (errors.Count > 0)
                throw new ErrorOnValidationException(errors);
        }
    }
}
