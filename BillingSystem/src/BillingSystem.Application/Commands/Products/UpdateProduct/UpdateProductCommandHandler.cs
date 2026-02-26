using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using BillingSystem.Domain.Interfaces.Data;
using BillingSystem.Domain.Interfaces.Repositories.Products;
using MediatR;

namespace BillingSystem.Application.Commands.Products.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(
            IProductWriteRepository productWriteRepository,
            IUnitOfWork unitOfWork
        )
        {
            _productWriteRepository = productWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productWriteRepository.GetByIdToUpdate(request.Id, cancellationToken)
                ?? throw new NotFoundException(ResourceMessagesException.PRODUCT_NOT_FOUND);

            request.UpdateDomain(product);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
