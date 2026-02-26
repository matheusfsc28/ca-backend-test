using BillingSystem.Domain.Entities.Base;
using BillingSystem.Domain.Interfaces.Repositories.Base;
using BillingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
                .AsNoTracking()
                .Where(e => ids.Contains(e.Id))
                .Select(e => e.Id)
                .ToHashSetAsync(cancellationToken);
        }

        public Task<bool> HasAnyById(Guid id, CancellationToken cancellationToken)
        {
            return _dbSet.AsNoTracking().AnyAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsNoTracking();

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
