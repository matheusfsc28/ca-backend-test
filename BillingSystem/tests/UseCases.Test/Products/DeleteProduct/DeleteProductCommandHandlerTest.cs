using BillingSystem.Application.Commands.Products.DeleteProduct;
using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using BillingSystem.Domain.Entities.Products;
using CommonTestUtilities.Commands.Products.DeleteProduct;
using CommonTestUtilities.Data.UnitOfWork;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Products;

namespace UseCases.Test.Products.DeleteProduct
{
    public class DeleteProductCommandHandlerTest
    {
        [Fact]
        public async Task Success()
        {
            var product = ProductBuilder.Build();
            var request = DeleteProductCommandBuilder.Build(product.Id);
            var handler = CreateHandler(product);

            await handler.Handle(request, CancellationToken.None);

            Assert.Equal(product.Id, request.Id);
            Assert.NotNull(product.DeletedAt);
        }

        [Fact]
        public async Task Error_Product_Not_Found()
        {
            var request = DeleteProductCommandBuilder.Build(Guid.NewGuid());
            var handler = CreateHandler();

            Func<Task> act = () => handler.Handle(request, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<NotFoundException>(act);
            Assert.Contains(ResourceMessagesException.PRODUCT_NOT_FOUND, exception.GetErrorMessages());
        }

        private static DeleteProductCommandHandler CreateHandler(Product? product = null)
        {
            var productWriteRepo = new ProductWriteRepositoryBuilder()
                .SetupGetByIdToUpdate(product)
                .Build();

            var unitOfWork = UnitOfWorkBuilder.Build();

            return new DeleteProductCommandHandler(productWriteRepo, unitOfWork);
        }
    }
}
