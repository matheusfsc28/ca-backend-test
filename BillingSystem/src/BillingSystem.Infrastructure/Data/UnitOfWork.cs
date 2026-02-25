using BillingSystem.Domain.Interfaces.Data;

namespace BillingSystem.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BillingSystemDbContext _context;

        public UnitOfWork(BillingSystemDbContext context) => _context = context;

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
