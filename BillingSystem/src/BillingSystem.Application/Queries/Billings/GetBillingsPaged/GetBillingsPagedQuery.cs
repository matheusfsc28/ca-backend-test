using BillingSystem.Application.DTOs.Requests.Base;
using BillingSystem.Application.DTOs.Requests.Billings;
using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Application.DTOs.Responses.Billings;
using MediatR;

namespace BillingSystem.Application.Queries.Billings.GetBillingsPaged
{
    public record GetBillingsPagedQuery(PaginationRequestDto<BillingRequestDto> RequestDto) : IRequest<PagedResponseDto<BillingResponseDto>>;
}
