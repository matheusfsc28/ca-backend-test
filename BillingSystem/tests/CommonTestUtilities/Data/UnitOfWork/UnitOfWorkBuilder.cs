using BillingSystem.Domain.Interfaces.Data;
using Moq;

namespace CommonTestUtilities.Data.UnitOfWork
{
    public class UnitOfWorkBuilder
    {
        public static IUnitOfWork Build() => new Mock<IUnitOfWork>().Object;
    }
}
