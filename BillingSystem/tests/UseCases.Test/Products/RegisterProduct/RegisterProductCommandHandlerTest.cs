using BillingSystem.Application.Commands.Products.RegisterProduct;
using BillingSystem.Application.DTOs.Responses.Products;
using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using BillingSystem.Domain.Interfaces.Repositories.Products;
using CommonTestUtilities.Commands.Products.RegisterProduct;
using CommonTestUtilities.Data.UnitOfWork;
using CommonTestUtilities.Repositories.Products;
using Moq;

namespace UseCases.Test.Products.RegisterProduct
{
    public class RegisterProductCommandHandlerTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RegisterProductCommandBuilder.Build();
            var handler = CreateHandler();

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(request.Id, result.Id);
            Assert.Equal(request.Name, result.Name);
            Assert.IsType<ProductResponseDto>(result);
        }

        [Fact]
        public async Task Error_Id_Already_Exists()
        {
            var request = RegisterProductCommandBuilder.Build();
            var handler = CreateHandler(idExists: true);

            Func<Task> act = async () => await handler.Handle(request, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
            Assert.Contains(exception.GetErrorMessages(), msg => msg.Contains(ResourceMessagesException.ID_ALREADY_EXISTS));
        }

        private static RegisterProductCommandHandler CreateHandler(bool idExists = false)
        {
            var productReadRepo = new ProductReadRepositoryBuilder()
                .SetupIdExists(idExists)
                .Build();

            var productWriteRepo = new Mock<IProductWriteRepository>().Object;
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new RegisterProductCommandHandler(
                productReadRepo,
                productWriteRepo,
                unitOfWork
            );
        }
    }
}
