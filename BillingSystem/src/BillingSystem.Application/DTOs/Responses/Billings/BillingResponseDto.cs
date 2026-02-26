using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Application.DTOs.Responses.Customers;
using BillingSystem.Domain.Entities.Billings;

namespace BillingSystem.Application.DTOs.Responses.Billings
{
    public class BillingResponseDto : BaseResponseDto
    {
        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public CustomerResponseDto Customer { get; set; }
        public IEnumerable<BillingLineResponseDto> BillingLines { get; set; }

        public BillingResponseDto(Billing billing)
        {
            Id = billing.Id;
            InvoiceNumber = billing.InvoiceNumber;
            Date = billing.Date;
            DueDate = billing.DueDate;
            TotalAmount = billing.TotalAmount;
            Currency = billing.Currency;
            Customer = new CustomerResponseDto(billing.Customer);
            BillingLines = billing.Lines.Select(l => new BillingLineResponseDto(l));
        }
    }
}
