using BillingSystem.Application.Commands.Products.UpdateProduct;
using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using BillingSystem.Domain.Entities.Products;
using CommonTestUtilities.Commands.Products.UpdateProduct;
using CommonTestUtilities.Data.UnitOfWork;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Products;
using Microsoft.VisualBasic;

namespace UseCases.Test.Products.UpdateProduct
{
    public class UpdateProductCommandHandlerTest
    {
        [Fact]
        public async Task Success()
        {
            var request = UpdateProductCommandBuilder.Build();
            var product = ProductBuilder.Build();
            var handler = CreateHandler(product);

            await handler.Handle(request, CancellationToken.None);
            
            Assert.Equal(request.RequestDto.Name, product.Name);
        }

        [Fact]
        public async Task Error_Product_Not_Found()
        {
            var request = UpdateProductCommandBuilder.Build();
            var handler = CreateHandler();

            Func<Task> act = async () => await handler.Handle(request, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<NotFoundException>(act);
            Assert.Contains(ResourceMessagesException.PRODUCT_NOT_FOUND, exception.GetErrorMessages());
        }

        private static UpdateProductCommandHandler CreateHandler(Product? product = null)
        {
            var productWriteRepo = new ProductWriteRepositoryBuilder()
                .SetupGetByIdToUpdate(product)
                .Build();

            var unitOfWork = UnitOfWorkBuilder.Build();

            return new UpdateProductCommandHandler(
                productWriteRepo,
                unitOfWork
            );
        }
    }
}
