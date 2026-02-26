using BillingSystem.Domain.Entities.Products;
using Bogus;

namespace CommonTestUtilities.Entities
{
    public class ProductBuilder
    {
        public static Product Build()
        {
            var fakerProduct = new Faker<Product>()
                .CustomInstantiator(f => 
                    new Product(Guid.NewGuid(), f.Commerce.ProductName())
                );

            return fakerProduct.Generate();
        }
    }
}
