using BillingSystem.Application.Commands.Customers.UpdateCustomer;
using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using BillingSystem.Domain.Entities.Customers;
using CommonTestUtilities.Commands.Customers.UpdateCustomer;
using CommonTestUtilities.Data.UnitOfWork;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Customers;

namespace UseCases.Test.Customers.UpdateCustomer
{
    public class UpdateCustomerCommandHandlerTest
    {
        [Fact]
        public async Task Success()
        {
            var request = UpdateCustomerCommandBuilder.Build();
            var customer = CustomerBuilder.Build();
            var handler = CreateHandler(customer);

            await handler.Handle(request, CancellationToken.None);

            Assert.Equal(request.RequestDto.Name, customer.Name);
            Assert.Equal(request.RequestDto.Email, customer.Email);
            Assert.Equal(request.RequestDto.Address, customer.Address);
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            var request = UpdateCustomerCommandBuilder.Build();
            var handler = CreateHandler(customer: null, emailExists: true);

            Func<Task> act = async () => await handler.Handle(request, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
            Assert.Contains(exception.GetErrorMessages(), msg => msg.Contains(ResourceMessagesException.EMAIL_ALREADY_EXISTS));
        }

        [Fact]
        public async Task Error_Customer_Not_Found()
        {
            var request = UpdateCustomerCommandBuilder.Build();
            var handler = CreateHandler(customer: null, emailExists: false);

            Func<Task> act = async () => await handler.Handle(request, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<NotFoundException>(act);
            Assert.Equal(ResourceMessagesException.CUSTOMER_NOT_FOUND, exception.Message);
        }

        private static UpdateCustomerCommandHandler CreateHandler(Customer? customer, bool emailExists = false)
        {
            var customerReadRepo = new CustomerReadRepositoryBuilder()
                .SetupEmailExists(emailExists)
                .Build();

            var customerWriteRepo = new CustomerWriteRepositoryBuilder()
                .SetupGetByIdToUpdate(customer)
                .Build();

            var unitOfWork = UnitOfWorkBuilder.Build();

            return new UpdateCustomerCommandHandler(customerReadRepo, customerWriteRepo, unitOfWork);
        }
    }
}
