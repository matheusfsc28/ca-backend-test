using BillingSystem.Domain.Entities.Billings;
using BillingSystem.Domain.Interfaces.Repositories.Billings;
using BillingSystem.Infrastructure.Data;
using BillingSystem.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace BillingSystem.Infrastructure.Repositories.Billings
{
    public class BillingRepository : BaseRepository<Billing>, IBillingWriteRepository, IBillingReadRepository
    {
        public BillingRepository(BillingSystemDbContext context) : base(context) { }

        public async Task<HashSet<string>> GetExistingInvoiceNumbersAsync(IEnumerable<string> InvoiceNumber, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Where(b => InvoiceNumber.Contains(b.InvoiceNumber))
                .Select(b => b.InvoiceNumber)
                .ToHashSetAsync(cancellationToken);
        }
    }
}
