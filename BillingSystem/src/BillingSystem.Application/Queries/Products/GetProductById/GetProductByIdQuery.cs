using BillingSystem.Application.DTOs.Responses.Products;
using MediatR;

namespace BillingSystem.Application.Queries.Products.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IRequest<ProductResponseDto>;
}
