using BillingSystem.Application.DTOs.Requests.Base;

namespace BillingSystem.Application.DTOs.Requests.Customers
{
    public class CustomerRequestDto : BaseRequestDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
