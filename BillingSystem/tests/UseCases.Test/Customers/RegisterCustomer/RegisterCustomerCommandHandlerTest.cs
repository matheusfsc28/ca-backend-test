using BillingSystem.Application.Commands.Customers.RegisterCustomer;
using BillingSystem.Application.DTOs.Responses.Customers;
using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using CommonTestUtilities.Commands.Customers.RegisterCustomer;
using CommonTestUtilities.Data.UnitOfWork;
using CommonTestUtilities.Repositories.Customers;
using Moq;

namespace UseCases.Test.Customers.RegisterCustomer
{
    public class RegisterCustomerCommandHandlerTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RegisterCustomerCommandBuilder.Build();
            var handler = CreateHandler();

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(request.Id, result.Id);
            Assert.Equal(request.Name, result.Name);
            Assert.Equal(request.Email, result.Email);
            Assert.IsType<CustomerResponseDto>(result);
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            var request = RegisterCustomerCommandBuilder.Build();
            var handler = CreateHandler(emailExists: true);

            Func<Task> act = async () => await handler.Handle(request, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
            Assert.Contains(exception.GetErrorMessages(), msg => msg.Contains(ResourceMessagesException.EMAIL_ALREADY_EXISTS));
        }

        [Fact]
        public async Task Error_Id_Already_Exists()
        {
            var request = RegisterCustomerCommandBuilder.Build();
            var handler = CreateHandler(emailExists: false, idExists: true);

            Func<Task> act = async () => await handler.Handle(request, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
            Assert.Contains(exception.GetErrorMessages(), msg => msg.Contains(ResourceMessagesException.ID_ALREADY_EXISTS));
        }

        private static RegisterCustomerCommandHandler CreateHandler(bool emailExists = false, bool idExists = false)
        {
            var customerReadRepo = new CustomerReadRepositoryBuilder()
                .SetupEmailExists(emailExists)
                .SetupIdExists(idExists)
                .Build();

            var customerWriteRepo = new Mock<BillingSystem.Domain.Interfaces.Repositories.Customers.ICustomerWriteRepository>().Object;
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new RegisterCustomerCommandHandler(
                customerReadRepo,
                customerWriteRepo,
                unitOfWork
            );
        }
    }
}
