using BillingSystem.Application.DTOs.Responses.ExternalBillingService;

namespace BillingSystem.Application.Abstractions.ExternalBillingService
{
    public interface IExternalBillingService
    {
        Task<IEnumerable<ExternalBillingResponseDto>> GetBillingsAsync(CancellationToken cancellationToken = default);
    }
}
