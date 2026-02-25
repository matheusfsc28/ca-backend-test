using BillingSystem.Domain.Entities.Billings;
using BillingSystem.Domain.Interfaces.Repositories.Billings;
using BillingSystem.Infrastructure.Data;
using BillingSystem.Infrastructure.Repositories.Write.Base;

namespace BillingSystem.Infrastructure.Repositories.Write.Billings
{
    public class BillingLineRepository : BaseRepository<BillingLine>, IBillingLineWriteRepository, IBillingLineReadRepository
    {
        public BillingLineRepository(BillingSystemDbContext context) : base(context) { }
    }
}
