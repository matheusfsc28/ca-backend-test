using BillingSystem.Application.DTOs.Responses.Billings;
using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using BillingSystem.Domain.Interfaces.Repositories.Billings;
using MediatR;

namespace BillingSystem.Application.Queries.Billings.GetBillingById
{
    public class GetBillingByIdQueryHandler : IRequestHandler<GetBillingByIdQuery, BillingResponseDto>
    {
        private readonly IBillingReadRepository _billingReadRepository;

        public GetBillingByIdQueryHandler(IBillingReadRepository billingReadRepository)
        {
            _billingReadRepository = billingReadRepository;
        }

        public async Task<BillingResponseDto> Handle(GetBillingByIdQuery request, CancellationToken cancellationToken)
        {
            var billing = await _billingReadRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(ResourceMessagesException.BILLING_NOT_FOUND);

            return new BillingResponseDto(billing);
        }
    }
}
