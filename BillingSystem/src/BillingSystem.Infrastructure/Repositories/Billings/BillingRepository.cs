using BillingSystem.Domain.Entities.Billings;
using BillingSystem.Domain.Interfaces.Repositories.Billings;
using BillingSystem.Infrastructure.Data;
using BillingSystem.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public override Task<Billing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _dbSet
                .AsNoTracking()
                .Include(b => b.Customer)
                .Include(b => b.Lines)
                    .ThenInclude(bl => bl.Product)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public override async Task<(IEnumerable<Billing> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<Billing, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Billing> query = _dbSet.AsNoTracking()
                .Include(b => b.Customer)
                .Include(b => b.Lines) 
                    .ThenInclude(l => l.Product);

            if (filter != null)
                query = query.Where(filter);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}
