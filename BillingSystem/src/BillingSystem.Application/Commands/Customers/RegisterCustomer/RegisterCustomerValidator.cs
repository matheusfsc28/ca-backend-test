using BillingSystem.Common.Exceptions;
using FluentValidation;

namespace BillingSystem.Application.Commands.Customers.RegisterCustomer
{
    public class RegisterCustomerValidator : AbstractValidator<RegisterCustomerCommand>
    {
        public RegisterCustomerValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
                .MaximumLength(150).WithMessage(string.Format(ResourceMessagesException.NAME_MAX_LENGTH, 150));

            RuleFor(request => request.Email)
                .NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY)
                .MaximumLength(150).WithMessage(string.Format(ResourceMessagesException.EMAIL_MAX_LENGTH, 150))
                .EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);

            RuleFor(request => request.Address)
                .NotEmpty().WithMessage(ResourceMessagesException.ADDRESS_EMPTY)
                .MaximumLength(250).WithMessage(string.Format(ResourceMessagesException.ADDRESS_MAX_LENGTH, 250));
        }
    }
}
