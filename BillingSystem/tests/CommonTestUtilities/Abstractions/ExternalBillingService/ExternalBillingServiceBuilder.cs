using BillingSystem.Application.Abstractions.ExternalBillingService;
using BillingSystem.Application.DTOs.Responses.ExternalBillingService;
using CommonTestUtilities.DTOs.Responses.ExternalBillingService;
using Moq;

namespace CommonTestUtilities.Abstractions.ExternalBillingService
{
    public class ExternalBillingServiceBuilder
    {
        private readonly Mock<IExternalBillingService> _service;

        public ExternalBillingServiceBuilder() => _service = new Mock<IExternalBillingService>();

        public ExternalBillingServiceBuilder ReturnsBillings(IEnumerable<ExternalBillingResponseDto>? billings = null)
        {
            billings ??= ExternalBillingResponseDtoBuilder.Build(2);

            _service.Setup(s => s.GetBillingsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(billings);
            return this;
        }

        public IExternalBillingService Build() => _service.Object;
    }
}
