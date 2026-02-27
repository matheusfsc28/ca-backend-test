using BillingSystem.Application.DTOs.Requests.Products;
using Bogus;

namespace CommonTestUtilities.DTOs.Requests.Products
{
    public class ProductRequestDtoBuilder
    {
        public static IEnumerable<ProductRequestDto> Build(int count = 1, int nameLength = 10)
        {
            var productRequestFaker = new Faker<ProductRequestDto>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName().PadRight(nameLength, 'a'));

            return productRequestFaker.Generate(count);
        }
    }
}
