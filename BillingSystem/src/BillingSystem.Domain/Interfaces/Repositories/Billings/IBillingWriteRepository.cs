using BillingSystem.Domain.Entities.Billings;
using BillingSystem.Domain.Interfaces.Repositories.Base;

namespace BillingSystem.Domain.Interfaces.Repositories.Billings
{
    public interface IBillingWriteRepository : IBaseWriteRepository<Billing>
    {
    }
}
