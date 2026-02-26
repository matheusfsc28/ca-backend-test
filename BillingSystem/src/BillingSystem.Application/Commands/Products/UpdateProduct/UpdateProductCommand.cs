using BillingSystem.Application.DTOs.Requests.Products;
using BillingSystem.Domain.Entities.Products;
using MediatR;

namespace BillingSystem.Application.Commands.Products.UpdateProduct
{
    public record UpdateProductCommand(
        Guid Id,
        ProductRequestDto RequestDto
    ) : IRequest
    {
        public void UpdateDomain(Product product)
        {
            product.Update(RequestDto.Name);
        }
    }
}
