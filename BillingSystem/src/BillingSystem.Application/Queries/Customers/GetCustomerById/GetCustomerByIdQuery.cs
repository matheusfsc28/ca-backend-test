using BillingSystem.Application.DTOs.Responses.Customers;
using MediatR;

namespace BillingSystem.Application.Queries.Customers.GetCustomerById
{
    public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerResponseDto>;
}
