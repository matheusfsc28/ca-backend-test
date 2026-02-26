using BillingSystem.Application.DTOs.Requests.Billings;
using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Application.DTOs.Responses.Billings;
using BillingSystem.Domain.Entities.Billings;
using BillingSystem.Domain.Interfaces.Repositories.Billings;
using MediatR;
using System.Linq.Expressions;

namespace BillingSystem.Application.Queries.Billings.GetBillingsPaged
{
    public class GetBillingsPagedQueryHandler : IRequestHandler<GetBillingsPagedQuery, PagedResponseDto<BillingResponseDto>>
    {
        private readonly IBillingReadRepository _billingReadRepository;

        public GetBillingsPagedQueryHandler(IBillingReadRepository billingReadRepository)
        {
            _billingReadRepository = billingReadRepository;
        }

        public async Task<PagedResponseDto<BillingResponseDto>> Handle(GetBillingsPagedQuery request, CancellationToken cancellationToken)
        {
            var filters = ApplyFilters(request.RequestDto.Filters);

            var pagedResult = await _billingReadRepository.GetPagedAsync(
                request.RequestDto.Page,
                request.RequestDto.PageSize,
                filters,
                cancellationToken
            );

            var dtos = pagedResult.Items.Select(b => new BillingResponseDto(b));

            return new PagedResponseDto<BillingResponseDto>(dtos, request.RequestDto.Page, request.RequestDto.PageSize, pagedResult.TotalCount);
        }

        private static Expression<Func<Billing, bool>>? ApplyFilters(BillingRequestDto? requestDto)
        {
            if (requestDto == null)
                return null;

            var invoiceNumberFilter = requestDto.InvoiceNumber?.ToLower();
            var currencyFilter = requestDto.InvoiceNumber?.ToLower();

            Expression<Func<Billing, bool>> filter = b =>
                (string.IsNullOrEmpty(invoiceNumberFilter) || b.InvoiceNumber.ToLower().Contains(invoiceNumberFilter)) &&
                (string.IsNullOrEmpty(currencyFilter) || b.Currency.ToLower().Contains(currencyFilter)) &&
                (requestDto.CustomerId == null || b.CustomerId == requestDto.CustomerId) &&
                (requestDto.Date == null || DateOnly.FromDateTime(b.Date) == DateOnly.FromDateTime(b.Date)) &&
                (requestDto.DueDate == null || DateOnly.FromDateTime(b.DueDate) == DateOnly.FromDateTime(b.DueDate));

            return filter;
        }
    }
}
