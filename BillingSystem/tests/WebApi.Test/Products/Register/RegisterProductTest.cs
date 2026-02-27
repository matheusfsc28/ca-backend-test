using BillingSystem.Application.Commands.Products.RegisterProduct;
using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Common.Exceptions;
using CommonTestUtilities.Commands.Products.RegisterProduct;
using CommonTestUtilities.DTOs.Requests.Products;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Products.Register
{
    public class RegisterProductTest : BillingSystemClassFixture
    {
        private const string ENDPOINT = "api/v1/product";

        public RegisterProductTest(CustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Success()
        {
            var request = RegisterProductCommandBuilder.Build();

            var response = await DoPost(ENDPOINT, request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var responseId = await response.Content.ReadFromJsonAsync<Guid>();
            Assert.NotEqual(Guid.Empty, responseId);

            Assert.NotNull(response.Headers.Location);
            Assert.Contains(responseId.ToString(), response.Headers.Location.ToString());
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Title(string culture)
        {
            var request = RegisterProductCommandBuilder.Build();
            request = request with { Name = string.Empty };

            var response = await DoPost(ENDPOINT, request, culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(expectedMessage, responseData.Errors);
        }
    }
}
