using BillingSystem.Application.Commands.Products.UpdateProduct;
using BillingSystem.Application.DTOs.Requests.Products;
using Bogus;

namespace CommonTestUtilities.Commands.Products.UpdateProduct
{
    public class UpdateProductCommandBuilder
    {
        public static UpdateProductCommand Build(Guid? id = null, int nameLength = 10)
        {
            return new Faker<UpdateProductCommand>()
                .CustomInstantiator(f => new UpdateProductCommand(
                    id ?? Guid.NewGuid(),
                    new ProductRequestDto(
                        f.Commerce.ProductName().PadRight(nameLength, 'a')
                    )
                ));
        }
    }
}
