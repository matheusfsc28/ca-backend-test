using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using BillingSystem.Domain.Interfaces.Data;
using BillingSystem.Domain.Interfaces.Repositories.Customers;
using MediatR;

namespace BillingSystem.Application.Commands.Customers.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly ICustomerWriteRepository _customerWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCustomerCommandHandler(
            ICustomerWriteRepository customerWriteRepository,
            IUnitOfWork unitOfWork
        )
        {
            _customerWriteRepository = customerWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerWriteRepository.GetByIdToUpdate(request.Id, cancellationToken)
                ?? throw new NotFoundException(ResourceMessagesException.CUSTOMER_NOT_FOUND);

            customer.Delete();

            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
