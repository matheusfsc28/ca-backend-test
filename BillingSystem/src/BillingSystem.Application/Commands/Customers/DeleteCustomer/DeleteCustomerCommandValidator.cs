using BillingSystem.Common.Exceptions;
using FluentValidation;

namespace BillingSystem.Application.Commands.Customers.DeleteCustomer
{
    public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);
        }
    }
}
