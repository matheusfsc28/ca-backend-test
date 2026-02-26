namespace BillingSystem.Application.DTOs.Responses.ExternalBillingService
{
    public class ExternalBillingLineResponseDto
    {
        public Guid ProductId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}
