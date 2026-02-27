using BillingSystem.Application.Abstractions.ExternalBillingService;
using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Application.DTOs.Responses.Billings;
using BillingSystem.Application.DTOs.Responses.ExternalBillingService;
using CommonTestUtilities.Abstractions.ExternalBillingService;
using CommonTestUtilities.DTOs.Responses.ExternalBillingService;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace WebApi.Test.Billings.Sync
{
    public class SyncBillingTest : BillingSystemClassFixture
    {
        private readonly Guid _customerId;
        private readonly Guid _productId;

        private const string ENDPOINT = "api/v1/billing/sync";

        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        public SyncBillingTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _customerId = factory.GetCustomersIds().Last();
            _productId = factory.GetProductsIds().Last();
        }

        [Fact]
        public async Task Success_All_Billings_Valid()
        {
            var billings = ExternalBillingResponseDtoBuilder.Build(2).ToList();
            var validCustomerId = _customerId;
            var validProductId = _productId;

            foreach (var b in billings)
            {
                b.Customer.Id = validCustomerId;
                foreach (var line in b.Lines) line.ProductId = validProductId;
            }

            var client = CreateClientWithCustomMock(billings);

            var response = await client.PostAsync(ENDPOINT, null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<SyncBillingsResponseDto>(_jsonOptions);
            Assert.NotNull(responseData);
            Assert.Equal(2, responseData.SyncedBillingsCount);
        }

        [Fact]
        public async Task Error_Validation_All_Billings_Invalid()
        {
            var billings = ExternalBillingResponseDtoBuilder.Build(2).ToList();

            var client = CreateClientWithCustomMock(billings);

            var response = await client.PostAsync(ENDPOINT, null);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var errorData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>(_jsonOptions);
            Assert.NotEmpty(errorData.Errors);
        }

        [Fact]
        public async Task Error_MultiStatus_Partial_Sync()
        {
            var billings = ExternalBillingResponseDtoBuilder.Build(2).ToList();

            billings[0].Customer.Id = _customerId;
            foreach (var line in billings[0].Lines) line.ProductId = _productId;

            var client = CreateClientWithCustomMock(billings);

            var response = await client.PostAsync(ENDPOINT, null);

            Assert.Equal(HttpStatusCode.MultiStatus, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorOnSyncBillingResponseDto>(_jsonOptions);

            Assert.NotNull(responseData);
            Assert.Equal(1, responseData.SyncedBillingsCount);
            Assert.NotEmpty(responseData.Errors);
        }

        private HttpClient CreateClientWithCustomMock(List<ExternalBillingResponseDto> mockedBillingsToReturn)
        {
            return _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IExternalBillingService));
                    if (descriptor != null) services.Remove(descriptor);

                    var specificMock = new ExternalBillingServiceBuilder()
                        .ReturnsBillings(mockedBillingsToReturn)
                        .Build();

                    services.AddScoped(provider => specificMock);
                });
            }).CreateClient();
        }
    }
}