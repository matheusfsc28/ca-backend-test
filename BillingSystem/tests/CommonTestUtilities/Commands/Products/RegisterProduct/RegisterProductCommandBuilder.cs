using BillingSystem.Application.Commands.Products.RegisterProduct;
using Bogus;

namespace CommonTestUtilities.Commands.Products.RegisterProduct
{
    public class RegisterProductCommandBuilder
    {
        public static RegisterProductCommand Build(int nameLength = 10)
        {
            return new Faker<RegisterProductCommand>()
                .CustomInstantiator(f => new RegisterProductCommand(
                    Guid.NewGuid(),
                    f.Commerce.ProductName().PadRight(nameLength, 'a')
                ));
        }
    }
}
