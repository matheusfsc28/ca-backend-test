namespace BillingSystem.Application.DTOs.Responses.ExternalBillingService
{
    public class ExternalBillingResponseDto
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public ExternalCustomerResponseDto Customer { get; set; } = new();
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public IEnumerable<ExternalBillingLineResponseDto> Lines { get; set; } = [];
        public bool IsValidForSync =>
            !string.IsNullOrEmpty(InvoiceNumber) &&
            Customer?.Id != null &&
            Lines != null &&
            Lines.Any();
    }
}
