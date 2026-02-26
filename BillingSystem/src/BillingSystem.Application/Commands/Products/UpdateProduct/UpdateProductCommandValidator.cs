using BillingSystem.Common.Exceptions;
using FluentValidation;

namespace BillingSystem.Application.Commands.Products.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

            When(request => request.RequestDto.Name != null, () =>
            {
                RuleFor(request => request.RequestDto.Name)
                    .NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY)
                    .MaximumLength(100).WithMessage(string.Format(ResourceMessagesException.NAME_MAX_LENGTH, 100));
            });
        }
    }
}
