using BillingSystem.Application.Commands.Products.DeleteProduct;
using Bogus;

namespace CommonTestUtilities.Commands.Products.DeleteProduct
{
    public class DeleteProductCommandBuilder
    {
        public static DeleteProductCommand Build(Guid? id = null)
        {
            var fakerCommand = new Faker<DeleteProductCommand>()
                .CustomInstantiator(f => new DeleteProductCommand(id ?? Guid.NewGuid()));

            return fakerCommand.Generate();
        }
    }
}
