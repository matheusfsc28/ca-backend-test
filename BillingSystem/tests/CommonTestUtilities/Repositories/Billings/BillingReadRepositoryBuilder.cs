using BillingSystem.Domain.Interfaces.Repositories.Billings;
using Moq;

namespace CommonTestUtilities.Repositories.Billings
{
    public class BillingReadRepositoryBuilder
    {
        private readonly Mock<IBillingReadRepository> _repository;

        public BillingReadRepositoryBuilder() => _repository = new Mock<IBillingReadRepository>();

        public BillingReadRepositoryBuilder ReturnsExistingInvoices(IEnumerable<string> invoiceNumbers)
        {
            _repository.Setup(repo => repo.GetExistingInvoiceNumbersAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(invoiceNumbers.ToHashSet());
            return this;
        }

        public IBillingReadRepository Build() => _repository.Object;
    }
}
