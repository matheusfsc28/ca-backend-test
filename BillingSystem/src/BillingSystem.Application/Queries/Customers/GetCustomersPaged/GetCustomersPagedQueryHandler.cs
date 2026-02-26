using BillingSystem.Application.DTOs.Requests.Customers;
using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Application.DTOs.Responses.Customers;
using BillingSystem.Domain.Entities.Customers;
using BillingSystem.Domain.Interfaces.Repositories.Customers;
using MediatR;
using System.Linq.Expressions;

namespace BillingSystem.Application.Queries.Customers.GetCustomersPaged
{
    public class GetCustomersPagedQueryHandler : IRequestHandler<GetCustomersPagedQuery, PagedResponseDto<CustomerResponseDto>>
    {
        private readonly ICustomerReadRepository _customerReadRepository;

        public GetCustomersPagedQueryHandler(ICustomerReadRepository customerReadRepository)
        {
            _customerReadRepository = customerReadRepository;
        }

        public async Task<PagedResponseDto<CustomerResponseDto>> Handle(GetCustomersPagedQuery request, CancellationToken cancellationToken)
        {
            var filters = ApplyFilters(request.RequestDto.Filters);

            var pagedResult = await _customerReadRepository.GetPagedAsync(
                request.RequestDto.Page,
                request.RequestDto.PageSize,
                filters,
                cancellationToken
            );

            var dtos = pagedResult.Items.Select(c => new CustomerResponseDto(c));

            return new PagedResponseDto<CustomerResponseDto>(dtos, request.RequestDto.Page, request.RequestDto.PageSize, pagedResult.TotalCount);
        }

        private static Expression<Func<Customer, bool>>? ApplyFilters(CustomerRequestDto? requestDto)
        {
            if (requestDto == null)
                return null;

            var nameFilter = requestDto.Name?.ToLower();
            var emailFilter = requestDto.Email?.ToLower();
            var addressFilter = requestDto.Address?.ToLower();

            Expression<Func<Customer, bool>> filter = c =>
                (string.IsNullOrEmpty(nameFilter) || c.Name.ToLower().Contains(nameFilter)) &&
                (string.IsNullOrEmpty(emailFilter) || c.Email.ToLower().Contains(emailFilter)) &&
                (string.IsNullOrEmpty(addressFilter) || c.Address.ToLower().Contains(addressFilter));

            return filter;
        }
    }
}
