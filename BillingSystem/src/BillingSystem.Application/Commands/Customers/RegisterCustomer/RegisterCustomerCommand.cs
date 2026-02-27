using BillingSystem.Application.DTOs.Responses.Customers;
using BillingSystem.Domain.Entities.Customers;
using MediatR;

namespace BillingSystem.Application.Commands.Customers.RegisterCustomer
{
    public record RegisterCustomerCommand(
        Guid Id,
        string Name,
        string Email,
        string Address
    ) : IRequest<Guid>
    {
        public Customer ToDomain()
        {
            return new Customer(
                Id, 
                Name, 
                Email, 
                Address
            );
        }
    }
}
