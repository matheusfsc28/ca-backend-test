using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Common.Exceptions;
using CommonTestUtilities.Commands.Customers.RegisterCustomer;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Customers.Register
{
    public class RegisterCustomerTest : BillingSystemClassFixture
    {
        private readonly string ENDPOINT = "api/v1/customer";

        public RegisterCustomerTest(CustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Success()
        {
            var request = RegisterCustomerCommandBuilder.Build();

            var response = await DoPost(ENDPOINT, request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseId = await response.Content.ReadFromJsonAsync<Guid>();

            Assert.NotEqual(Guid.Empty, responseId);
            Assert.Equal(request.Id, responseId);

            Assert.NotNull(response.Headers.Location);
            Assert.Contains(responseId.ToString(), response.Headers.Location.ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string culture)
        {
            var request = RegisterCustomerCommandBuilder.Build();
            request = request with { Name = string.Empty };

            var response = await DoPost(ENDPOINT, request, culture);

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
            var request = RegisterCustomerCommandBuilder.Build(nameLength: 151);

            var response = await DoPost(ENDPOINT, request, culture);

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
            var request = RegisterCustomerCommandBuilder.Build();
            request = request with { Email = string.Empty };

            var response = await DoPost(ENDPOINT, request, culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_EMPTY", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(expectedMessage, responseData.Errors);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Max_Legth_Email(string culture)
        {
            var request = RegisterCustomerCommandBuilder.Build(emailLenght: 151);

            var response = await DoPost(ENDPOINT, request, culture);

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
            var request = RegisterCustomerCommandBuilder.Build();
            request = request with { Address = string.Empty };

            var response = await DoPost(ENDPOINT, request, culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("ADDRESS_EMPTY", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(expectedMessage, responseData.Errors);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Max_Legth_Address(string culture)
        {
            var request = RegisterCustomerCommandBuilder.Build(addressLength: 251);

            var response = await DoPost(ENDPOINT, request, culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("ADDRESS_MAX_LENGTH", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(string.Format(expectedMessage!, 250), responseData.Errors);
        }
    }
}
