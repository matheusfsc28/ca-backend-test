using System.Text.Json.Serialization;

namespace BillingSystem.Application.DTOs.Responses.ExternalBillingService
{
    public class ExternalCustomerResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
