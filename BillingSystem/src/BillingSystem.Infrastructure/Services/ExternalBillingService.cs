using BillingSystem.Application.Abstractions.ExternalBillingService;
using BillingSystem.Application.DTOs.Responses.ExternalBillingService;
using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using System.Net.Http.Json;

namespace BillingSystem.Infrastructure.Services
{
    public class ExternalBillingService : IExternalBillingService
    {
        private readonly HttpClient _client;

        private const string LIST_BILLING_ENDPOINT = "api/v1/billing";

        public ExternalBillingService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<ExternalBillingResponseDto>> GetBillingsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _client.GetAsync(LIST_BILLING_ENDPOINT, cancellationToken);

                response.EnsureSuccessStatusCode();

                var billings = await response.Content.ReadFromJsonAsync<IEnumerable<ExternalBillingResponseDto>>(cancellationToken);

                return billings ?? [];
            }
            catch (HttpRequestException)
            {
                throw new ExternalServiceException(ResourceMessagesException.EXTERNAL_API_ERROR);
            }
            catch (TaskCanceledException)
            {
                throw new ExternalServiceException(ResourceMessagesException.EXTERNAL_API_TIMEOUT);
            }
            catch (Exception)
            {
                throw new ExternalServiceException(ResourceMessagesException.EXTERNAL_API_UNKNOWN_ERROR);
            }
        }
    }
}
