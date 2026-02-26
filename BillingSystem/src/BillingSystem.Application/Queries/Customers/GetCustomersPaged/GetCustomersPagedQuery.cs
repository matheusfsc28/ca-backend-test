using BillingSystem.Application.DTOs.Requests.Base;
using BillingSystem.Application.DTOs.Requests.Customers;
using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Application.DTOs.Responses.Customers;
using MediatR;

namespace BillingSystem.Application.Queries.Customers.GetCustomersPaged
{
    public record GetCustomersPagedQuery(PaginationRequestDto<CustomerRequestDto> RequestDto) : IRequest<PagedResponseDto<CustomerResponseDto>>;
}
