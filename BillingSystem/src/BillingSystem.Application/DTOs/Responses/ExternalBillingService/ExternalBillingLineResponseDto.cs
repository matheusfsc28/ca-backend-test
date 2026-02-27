using System.Text.Json.Serialization;

namespace BillingSystem.Application.DTOs.Responses.ExternalBillingService
{
    public class ExternalBillingLineResponseDto
    {
        [JsonPropertyName("productId")]
        public Guid ProductId { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
        [JsonPropertyName("unit_price")]
        public decimal UnitPrice { get; set; }
        [JsonPropertyName("subtotal")]
        public decimal Subtotal { get; set; }
    }
}
