using BillingSystem.Domain.Entities.Products;
using BillingSystem.Domain.Interfaces.Repositories.Products;
using Moq;

namespace CommonTestUtilities.Repositories.Products
{
    public class ProductWriteRepositoryBuilder
    {
        private readonly Mock<IProductWriteRepository> _repository;

        public ProductWriteRepositoryBuilder() => _repository = new Mock<IProductWriteRepository>();

        public ProductWriteRepositoryBuilder SetupGetByIdToUpdate(Product? product)
        {
            _repository.Setup(repo => repo.GetByIdToUpdate(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);
            return this;
        }

        public IProductWriteRepository Build() => _repository.Object;
    }
}
