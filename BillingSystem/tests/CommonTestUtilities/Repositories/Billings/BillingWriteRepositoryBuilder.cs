using BillingSystem.Domain.Interfaces.Repositories.Billings;
using Moq;

namespace CommonTestUtilities.Repositories.Billings
{
    public class BillingWriteRepositoryBuilder
    {
        public static IBillingWriteRepository Build() => new Mock<IBillingWriteRepository>().Object;
    }
}
