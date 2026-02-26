using BillingSystem.Common.Exceptions;
using FluentValidation;

namespace BillingSystem.Application.Commands.Customers.UpdateCustomer
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

            When(request => request.RequestDto.Name != null, () => {

                RuleFor(request => request.RequestDto.Name)
                    .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
                    .MaximumLength(150).WithMessage(string.Format(ResourceMessagesException.NAME_MAX_LENGTH, 150));

            });

            When(request => request.RequestDto.Email != null, () => {

                RuleFor(request => request.RequestDto.Email)
                    .NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY)
                    .MaximumLength(150).WithMessage(string.Format(ResourceMessagesException.EMAIL_MAX_LENGTH, 150));

            });

            When(request => request.RequestDto.Address != null, () => {

                RuleFor(request => request.RequestDto.Address)
                    .NotEmpty().WithMessage(ResourceMessagesException.ADDRESS_EMPTY)
                    .MaximumLength(250).WithMessage(string.Format(ResourceMessagesException.ADDRESS_MAX_LENGTH, 250));

            });
        }
    }
}
