using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Domain.Entities.Products;

namespace BillingSystem.Application.DTOs.Responses.Products
{
    public class ProductResponseDto : BaseResponseDto
    {
        public string Name { get; set; }

        public ProductResponseDto(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            CreatedAt = product.CreatedAt;
            UpdatedAt = product.UpdatedAt;
            DeletedAt = product.DeletedAt;
        }
    }
}
