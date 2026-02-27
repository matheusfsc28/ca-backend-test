using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using BillingSystem.Domain.Interfaces.Data;
using BillingSystem.Domain.Interfaces.Repositories.Products;
using MediatR;

namespace BillingSystem.Application.Commands.Products.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(
            IProductWriteRepository productWriteRepository,
            IUnitOfWork unitOfWork
        )
        {
            _productWriteRepository = productWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productWriteRepository.GetByIdToUpdate(request.Id, cancellationToken)
                ?? throw new NotFoundException(ResourceMessagesException.PRODUCT_NOT_FOUND);

            product.Delete();

            await _unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
