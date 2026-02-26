using BillingSystem.Domain.Interfaces.Repositories.Billings;
using Moq;

namespace CommonTestUtilities.Repositories.Billings
{
    public class BillingLineWriteRepositoryBuilder
    {
        public static IBillingLineWriteRepository Build() => new Mock<IBillingLineWriteRepository>().Object;
    }
}
