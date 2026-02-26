using BillingSystem.Application.DTOs.Responses.Customers;
using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using BillingSystem.Domain.Interfaces.Repositories.Customers;
using MediatR;

namespace BillingSystem.Application.Queries.Customers.GetCustomerById
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerResponseDto>
    {
        private readonly ICustomerReadRepository _customerReadRepository;

        public GetCustomerByIdQueryHandler(ICustomerReadRepository customerReadRepository)
        {
            _customerReadRepository = customerReadRepository;
        }

        public async Task<CustomerResponseDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerReadRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(ResourceMessagesException.CUSTOMER_NOT_FOUND);

            return new CustomerResponseDto(customer);
        }
    }
}
