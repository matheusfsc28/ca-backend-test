using BillingSystem.Application.DTOs.Requests.Base;

namespace BillingSystem.Application.DTOs.Requests.Billings
{
    public class BillingRequestDto : BaseRequestDto
    {
        public string? InvoiceNumber { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Currency { get; set; }
    }
}
