using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Common.Exceptions;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Customers.Delete
{
    public class DeleteCustomerTest : BillingSystemClassFixture
    {
        private readonly Guid _customerId;
        private readonly string ENDPOINT = "api/v1/customer";

        public DeleteCustomerTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _customerId = factory.GetCustomersIds().First();
        }

        [Fact]
        public async Task Success()
        {
            var response = await DoDelete(FormattedEndpoint());

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            response = await DoGet(FormattedEndpoint());

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Customer_Not_Found(string culture)
        {
            var response = await DoDelete(FormattedEndpoint(Guid.NewGuid()), culture);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("CUSTOMER_NOT_FOUND", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(expectedMessage, responseData.Errors);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Guid_Empty(string culture)
        {
            var response = await DoDelete(FormattedEndpoint(Guid.Empty), culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("ID_EMPTY", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(expectedMessage, responseData.Errors);
        }

        private string FormattedEndpoint(Guid? id = null)
        {
            return $"{ENDPOINT}/{id ?? _customerId}";
        }
    }
}
