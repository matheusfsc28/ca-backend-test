using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Common.Exceptions;
using CommonTestUtilities.DTOs.Requests.Customers;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Customers.Update
{
    public class UpdateCustomerTest : BillingSystemClassFixture
    {
        private readonly Guid _customerId;
        private readonly string ENDPOINT = "api/v1/customer";

        public UpdateCustomerTest(CustomWebApplicationFactory factory) : base(factory) 
        {
            _customerId = factory.GetCustomersIds().First();
        }

        [Fact]
        public async Task Success()
        {
            var request = CustomerRequestDtoBuilder.Build();

            var response = await DoPut(FormattedEndpoint(), request.First());

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Not_Found_Customer(string culture)
        {
            var request = CustomerRequestDtoBuilder.Build();

            var response = await DoPut(FormattedEndpoint(Guid.NewGuid()), request.First(), culture);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("CUSTOMER_NOT_FOUND", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(expectedMessage, responseData.Errors);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string culture)
        {
            var requests = CustomerRequestDtoBuilder.Build();
            var request = requests.First();
            request.Name = string.Empty;

            var response = await DoPut(FormattedEndpoint(), request, culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(expectedMessage, responseData.Errors);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Max_Length_Name(string culture)
        {
            var requests = CustomerRequestDtoBuilder.Build(nameLength: 151);
            var request = requests.First();

            var response = await DoPut(FormattedEndpoint(), request, culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_MAX_LENGTH", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(string.Format(expectedMessage!, 150), responseData.Errors);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Email(string culture)
        {
            var requests = CustomerRequestDtoBuilder.Build();
            var request = requests.First();
            request.Email = string.Empty;

            var response = await DoPut(FormattedEndpoint(), request, culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_EMPTY", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(expectedMessage, responseData.Errors);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Max_Length_Email(string culture)
        {
            var requests = CustomerRequestDtoBuilder.Build(emailLength: 151);
            var request = requests.First();

            var response = await DoPut(FormattedEndpoint(), request, culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_MAX_LENGTH", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(string.Format(expectedMessage!, 150), responseData.Errors);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Address(string culture)
        {
            var requests = CustomerRequestDtoBuilder.Build();
            var request = requests.First();
            request.Address = string.Empty;

            var response = await DoPut(FormattedEndpoint(), request, culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("ADDRESS_EMPTY", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(expectedMessage, responseData.Errors);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Max_Length_Address(string culture)
        {
            var requests = CustomerRequestDtoBuilder.Build(addressLength: 251);
            var request = requests.First();

            var response = await DoPut(FormattedEndpoint(), request, culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("ADDRESS_MAX_LENGTH", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(string.Format(expectedMessage!, 250), responseData.Errors);
        }

        private string FormattedEndpoint(Guid? id = null)
        {
            return $"{ENDPOINT}/{id ?? _customerId}";
        }
    }
}
