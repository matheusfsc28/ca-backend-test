using BillingSystem.Domain.Entities.Base;
using BillingSystem.Domain.Interfaces.Repositories.Base;
using BillingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BillingSystem.Infrastructure.Repositories.Base
{
    public class BaseRepository<T> : IBaseReadRepository<T>, IBaseWriteRepository<T> where T : BaseEntity
    {
        protected readonly BillingSystemDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(BillingSystemDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task AddAsync(IEnumerable<T> entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entity, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public Task<T?> GetByIdToUpdate(Guid id, CancellationToken cancellationToken)
        {
            return _dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<HashSet<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(e => ids.Contains(e.Id))
                .Select(e => e.Id)
                .ToHashSetAsync(cancellationToken);
        }

        public Task<bool> HasAnyById(Guid id, CancellationToken cancellationToken)
        {
            return _dbSet.AnyAsync(e => e.Id == id, cancellationToken);
        }
    }
}
