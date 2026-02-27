using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Common.Exceptions;
using CommonTestUtilities.DTOs.Requests.Products;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Products.Update
{
    public class UpdateProductTest : BillingSystemClassFixture
    {
        private readonly Guid _productId;
        private const string ENDPOINT = "api/v1/product";

        public UpdateProductTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _productId = factory.GetProductsIds().First();
        }

        [Fact]
        public async Task Success()
        {
            var request = ProductRequestDtoBuilder.Build().First();

            var response = await DoPut(FormattedEndpoint(), request);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Product_Not_Found(string culture)
        {
            var request = ProductRequestDtoBuilder.Build().First();

            var response = await DoPut(FormattedEndpoint(Guid.NewGuid()), request, culture);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("PRODUCT_NOT_FOUND", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(expectedMessage, responseData.Errors);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Product_Max_Length_Name(string culture)
        {
            var request = ProductRequestDtoBuilder.Build(nameLength: 101).First();

            var response = await DoPut(FormattedEndpoint(), request, culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_MAX_LENGTH", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(string.Format(expectedMessage!, 100), responseData.Errors);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Product_Name_Empty(string culture)
        {
            var request = ProductRequestDtoBuilder.Build().First();
            request.Name = string.Empty;

            var response = await DoPut(FormattedEndpoint(), request, culture);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseData = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            Assert.NotNull(responseData);
            Assert.Contains(expectedMessage, responseData.Errors);
        }

        private string FormattedEndpoint(Guid? id = null)
        {
            return $"{ENDPOINT}/{id ?? _productId}";
        }
    }
}
