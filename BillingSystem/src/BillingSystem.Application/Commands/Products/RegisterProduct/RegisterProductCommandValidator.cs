using BillingSystem.Common.Exceptions;
using FluentValidation;

namespace BillingSystem.Application.Commands.Products.RegisterProduct
{
    public class RegisterProductCommandValidator : AbstractValidator<RegisterProductCommand>
    {
        public RegisterProductCommandValidator() 
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

            RuleFor(request => request.Name)
                .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
                .MaximumLength(100).WithMessage(string.Format(ResourceMessagesException.NAME_MAX_LENGTH, 100));
        }
    }
}
