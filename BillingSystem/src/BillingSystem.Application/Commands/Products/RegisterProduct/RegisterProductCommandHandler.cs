using BillingSystem.Application.DTOs.Responses.Products;
using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using BillingSystem.Domain.Interfaces.Data;
using BillingSystem.Domain.Interfaces.Repositories.Products;
using MediatR;

namespace BillingSystem.Application.Commands.Products.RegisterProduct
{
    public class RegisterProductCommandHandler : IRequestHandler<RegisterProductCommand, ProductResponseDto>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterProductCommandHandler(
            IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository,
            IUnitOfWork unitOfWork
        )
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductResponseDto> Handle(RegisterProductCommand request, CancellationToken cancellationToken)
        {
            await CheckIfProductIsUniqueAsync(request, cancellationToken);

            var product = request.ToDomain();

            await _productWriteRepository.AddAsync(product, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return new ProductResponseDto(product);
        }

        private async Task CheckIfProductIsUniqueAsync(RegisterProductCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            if (await _productReadRepository.HasAnyById(request.Id, cancellationToken))
                errors.Add(ResourceMessagesException.ID_ALREADY_EXISTS);

            if (errors.Count > 0)
                throw new ErrorOnValidationException(errors);
        }
    }
}
