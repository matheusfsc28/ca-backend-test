using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using BillingSystem.Domain.Interfaces.Data;
using BillingSystem.Domain.Interfaces.Repositories.Customers;
using MediatR;

namespace BillingSystem.Application.Commands.Customers.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
    {
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerCommandHandler(
            ICustomerReadRepository customerReadRepository,
            ICustomerWriteRepository customerWriteRepository,
            IUnitOfWork unitOfWork
        )
        {
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            await CheckIfCustomerIsUniqueAsync(request, cancellationToken);

            var customer = await _customerWriteRepository.GetByIdToUpdate(request.Id, cancellationToken)
                ?? throw new NotFoundException(ResourceMessagesException.CUSTOMER_NOT_FOUND);

            request.UpdateDomain(customer);

            await _unitOfWork.CommitAsync(cancellationToken);
        }

        private async Task CheckIfCustomerIsUniqueAsync(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            if (request.RequestDto.Email != null && await _customerReadRepository.EmailRegistered(request.Id, request.RequestDto.Email, cancellationToken))
                errors.Add(ResourceMessagesException.EMAIL_ALREADY_EXISTS);

            if (errors.Count > 0)
                throw new ErrorOnValidationException(errors);
        }
    }
}
