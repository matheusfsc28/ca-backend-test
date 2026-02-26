using BillingSystem.Application.DTOs.Requests.Customers;
using BillingSystem.Domain.Entities.Customers;
using MediatR;

namespace BillingSystem.Application.Commands.Customers.UpdateCustomer
{
    public record UpdateCustomerCommand(
        Guid Id,
        CustomerRequestDto RequestDto
    ) : IRequest
    {
        public void UpdateDomain(Customer customer)
        {
            customer.Update(RequestDto.Name, RequestDto.Email, RequestDto.Address);
        }
    }
}
