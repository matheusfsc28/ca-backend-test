using BillingSystem.Application.Commands.Products.RegisterProduct;
using BillingSystem.Application.DTOs.Responses.Products;
using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using CommonTestUtilities.Commands.Products.RegisterProduct;
using CommonTestUtilities.Data.UnitOfWork;
using CommonTestUtilities.Repositories.Products;

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

            Assert.IsType<Guid>(result);
            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal(request.Id, result);
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

            var productWriteRepo = new ProductWriteRepositoryBuilder().Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new RegisterProductCommandHandler(
                productReadRepo,
                productWriteRepo,
                unitOfWork
            );
        }
    }
}
