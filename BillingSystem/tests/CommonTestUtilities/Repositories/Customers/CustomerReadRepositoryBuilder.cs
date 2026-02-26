using BillingSystem.Domain.Interfaces.Repositories.Customers;
using Moq;

namespace CommonTestUtilities.Repositories.Customers
{
    public class CustomerReadRepositoryBuilder
    {
        private readonly Mock<ICustomerReadRepository> _repository;

        public CustomerReadRepositoryBuilder() => _repository = new Mock<ICustomerReadRepository>();

        public CustomerReadRepositoryBuilder ReturnsExistingIds(IEnumerable<Guid> ids)
        {
            _repository.Setup(repo => repo.GetExistingIdsAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ids.ToHashSet());
            return this;
        }

        public CustomerReadRepositoryBuilder SetupEmailExists(bool exists)
        {
            _repository.Setup(repo => repo.EmailRegistered(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(exists);

            _repository.Setup(repo => repo.EmailRegistered(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(exists);

            return this;
        }

        public CustomerReadRepositoryBuilder SetupIdExists(bool exists)
        {
            _repository.Setup(repo => repo.HasAnyById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(exists);
            return this;
        }

        public ICustomerReadRepository Build() => _repository.Object;
    }
}
