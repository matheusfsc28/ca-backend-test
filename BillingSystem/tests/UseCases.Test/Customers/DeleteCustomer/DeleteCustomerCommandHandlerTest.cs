using BillingSystem.Application.Commands.Customers.DeleteCustomer;
using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using BillingSystem.Domain.Entities.Customers;
using CommonTestUtilities.Commands.Customers.DeleteCustomer;
using CommonTestUtilities.Data.UnitOfWork;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Customers;

namespace UseCases.Test.Customers.DeleteCustomer
{
    public class DeleteCustomerCommandHandlerTest
    {
        [Fact]
        public async Task Success()
        {
            var customer = CustomerBuilder.Build();
            var request = DeleteCustomerCommandBuilder.Build(customer.Id);
            var handler = CreateHandler(customer);

            await handler.Handle(request, CancellationToken.None);

            Assert.Equal(request.Id, customer.Id);
            Assert.NotNull(customer.DeletedAt);
        }

        [Fact]
        public async Task Error_Customer_Not_Found()
        {
            var request = DeleteCustomerCommandBuilder.Build(Guid.NewGuid());
            var handler = CreateHandler();

            Func<Task> act = () => handler.Handle(request, CancellationToken.None);

            var exception = await Assert.ThrowsAsync<NotFoundException>(act);
            Assert.Equal(ResourceMessagesException.CUSTOMER_NOT_FOUND, exception.Message);
        }

        private static DeleteCustomerCommandHandler CreateHandler(Customer? customer = null)
        {
            var customerWriteRepo = new CustomerWriteRepositoryBuilder()
                .SetupGetByIdToUpdate(customer)
                .Build();

            var unitOfWork = UnitOfWorkBuilder.Build();

            return new DeleteCustomerCommandHandler(customerWriteRepo, unitOfWork);
        }
    }
}
