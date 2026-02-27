using System.Text.Json.Serialization;

namespace BillingSystem.Application.DTOs.Responses.ExternalBillingService
{
    public class ExternalBillingResponseDto
    {
        [JsonPropertyName("invoice_number")]
        public string InvoiceNumber { get; set; } = string.Empty;
        [JsonPropertyName("customer")]
        public ExternalCustomerResponseDto Customer { get; set; } = new();
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("due_date")]
        public DateTime DueDate { get; set; }
        [JsonPropertyName("total_amount")]
        public decimal TotalAmount { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;
        [JsonPropertyName("lines")]
        public IEnumerable<ExternalBillingLineResponseDto> Lines { get; set; } = [];

        public bool IsValidForSync =>
            !string.IsNullOrEmpty(InvoiceNumber) &&
            Customer?.Id != null &&
            Lines != null &&
            Lines.Any();
    }
}
