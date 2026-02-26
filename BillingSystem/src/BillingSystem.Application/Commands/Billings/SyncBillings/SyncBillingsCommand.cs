using BillingSystem.Application.DTOs.Responses.Billings;
using MediatR;

namespace BillingSystem.Application.Commands.Billings.SyncBillings
{
    public record SyncBillingsCommand : IRequest<SyncBillingsResponseDto>;
}
