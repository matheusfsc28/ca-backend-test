using BillingSystem.Common.Exceptions;
using FluentValidation;

namespace BillingSystem.Application.Commands.Products.DeleteProduct
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator() 
        {
            RuleFor(request => request.Id)
                .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);
        }
    }
}
