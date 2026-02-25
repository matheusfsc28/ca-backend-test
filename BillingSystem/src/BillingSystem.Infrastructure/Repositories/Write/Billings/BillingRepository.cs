using BillingSystem.Domain.Entities.Billings;
using BillingSystem.Domain.Interfaces.Repositories.Billings;
using BillingSystem.Infrastructure.Data;
using BillingSystem.Infrastructure.Repositories.Write.Base;

namespace BillingSystem.Infrastructure.Repositories.Write.Billings
{
    public class BillingRepository : BaseRepository<Billing>, IBillingWriteRepository, IBillingReadRepository
    {
        public BillingRepository(BillingSystemDbContext context) : base(context) { }
    }
}
