using MediatR;

namespace BillingSystem.Application.Commands.Customers.DeleteCustomer
{
    public record DeleteCustomerCommand(Guid Id) : IRequest;
}
