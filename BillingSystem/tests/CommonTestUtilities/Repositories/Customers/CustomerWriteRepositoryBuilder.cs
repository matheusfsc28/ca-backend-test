using BillingSystem.Domain.Entities.Customers;
using BillingSystem.Domain.Interfaces.Repositories.Customers;
using Moq;

namespace CommonTestUtilities.Repositories.Customers
{
    public class CustomerWriteRepositoryBuilder
    {
        private readonly Mock<ICustomerWriteRepository> _repository;

        public CustomerWriteRepositoryBuilder() => _repository = new Mock<ICustomerWriteRepository>();

        public CustomerWriteRepositoryBuilder SetupGetByIdToUpdate(Customer? customer)
        {
            _repository.Setup(repo => repo.GetByIdToUpdate(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);
            return this;
        }

        public ICustomerWriteRepository Build() => _repository.Object;
    }
}
