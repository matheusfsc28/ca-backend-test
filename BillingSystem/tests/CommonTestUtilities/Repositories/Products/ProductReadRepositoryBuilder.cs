using BillingSystem.Domain.Interfaces.Repositories.Products;
using Moq;

namespace CommonTestUtilities.Repositories.Products
{
    public class ProductReadRepositoryBuilder
    {
        private readonly Mock<IProductReadRepository> _repository;

        public ProductReadRepositoryBuilder() => _repository = new Mock<IProductReadRepository>();

        public ProductReadRepositoryBuilder ReturnsExistingIds(IEnumerable<Guid> ids)
        {
            _repository.Setup(repo => repo.GetExistingIdsAsync(It.IsAny<IEnumerable<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ids.ToHashSet());
            return this;
        }

        public IProductReadRepository Build() => _repository.Object;
    }
}
