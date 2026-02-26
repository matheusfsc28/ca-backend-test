using BillingSystem.Application.DTOs.Responses.Customers;
using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using BillingSystem.Domain.Interfaces.Data;
using BillingSystem.Domain.Interfaces.Repositories.Customers;
using MediatR;

namespace BillingSystem.Application.Commands.Customers.RegisterCustomer
{
    public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, CustomerResponseDto>
    {
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCustomerCommandHandler(
            ICustomerReadRepository customerReadRepository,
            ICustomerWriteRepository customerWriteRepository,
            IUnitOfWork unitOfWork
        )
        {
            _customerReadRepository = customerReadRepository;
            _customerWriteRepository = customerWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerResponseDto> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            await CheckIfCustomerIsUniqueAsync(request, cancellationToken);

            var customer = request.ToDomain();

            await _customerWriteRepository.AddAsync(customer, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return new CustomerResponseDto(customer);
        }

        private async Task CheckIfCustomerIsUniqueAsync(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            if (await _customerReadRepository.EmailRegistered(request.Email, cancellationToken))
                errors.Add(ResourceMessagesException.EMAIL_ALREADY_EXISTS);

            if (await _customerReadRepository.HasAnyById(request.Id, cancellationToken))
                errors.Add(ResourceMessagesException.ID_ALREADY_EXISTS);

            if (errors.Count > 0)
                throw new ErrorOnValidationException(errors);
        }
    }
}
