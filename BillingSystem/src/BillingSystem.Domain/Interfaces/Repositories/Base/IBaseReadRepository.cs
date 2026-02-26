using BillingSystem.Domain.Entities.Base;
using System.Linq.Expressions;

namespace BillingSystem.Domain.Interfaces.Repositories.Base
{
    public interface IBaseReadRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> HasAnyById(Guid id, CancellationToken cancellationToken);
        Task<HashSet<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);
    }
}
