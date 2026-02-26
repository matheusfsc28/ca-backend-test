using BillingSystem.Application.DTOs.Responses.Products;
using BillingSystem.Domain.Entities.Products;
using MediatR;

namespace BillingSystem.Application.Commands.Products.RegisterProduct
{
    public record RegisterProductCommand(
        Guid Id,
        string Name
    ) : IRequest<ProductResponseDto>
    {
        public Product ToDomain()
        {
            return new Product(Id, Name);
        }
    }
}
