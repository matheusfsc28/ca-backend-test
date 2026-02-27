using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test
{
    public class BillingSystemClassFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        protected readonly CustomWebApplicationFactory _factory;


        public BillingSystemClassFixture(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
            _factory = factory;
        }

        protected async Task<HttpResponseMessage> DoPost(string endpoint, object request, string culture = "en")
        {
            ChangeRequestCulture(culture);

            return await _httpClient.PostAsJsonAsync(endpoint, request);
        }

        protected async Task<HttpResponseMessage> DoPost(string endpoint, string culture = "en")
        {
            ChangeRequestCulture(culture);

            return await _httpClient.PostAsync(endpoint, null);
        }

        protected async Task<HttpResponseMessage> DoGet(string endpoint, string culture = "en")
        {
            ChangeRequestCulture(culture);
            return await _httpClient.GetAsync(endpoint);
        }

        protected async Task<HttpResponseMessage> DoPut(string endpoint, object request,  string culture = "en")
        {
            ChangeRequestCulture(culture);
            return await _httpClient.PutAsJsonAsync(endpoint, request);
        }

        protected async Task<HttpResponseMessage> DoDelete(string endpoint, string culture = "en")
        {
            ChangeRequestCulture(culture);
            return await _httpClient.DeleteAsync(endpoint);
        }

        private void ChangeRequestCulture(string culture)
        {
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
        }
    }
}
