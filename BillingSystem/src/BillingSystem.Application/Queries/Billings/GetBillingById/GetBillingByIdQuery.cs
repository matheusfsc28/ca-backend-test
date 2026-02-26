using BillingSystem.Application.DTOs.Responses.Billings;
using MediatR;

namespace BillingSystem.Application.Queries.Billings.GetBillingById
{
    public record GetBillingByIdQuery(Guid Id) : IRequest<BillingResponseDto>;
}
