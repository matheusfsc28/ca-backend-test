using MediatR;

namespace BillingSystem.Application.Commands.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : IRequest;
}
