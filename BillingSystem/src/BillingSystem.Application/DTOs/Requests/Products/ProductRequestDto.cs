using BillingSystem.Application.DTOs.Requests.Base;

namespace BillingSystem.Application.DTOs.Requests.Products
{
    public class ProductRequestDto : BaseRequestDto
    {
        public string? Name { get; set; }
    }
}
