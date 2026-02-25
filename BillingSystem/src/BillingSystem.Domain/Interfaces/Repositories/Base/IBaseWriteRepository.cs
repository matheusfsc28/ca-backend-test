using BillingSystem.Domain.Entities.Base;

namespace BillingSystem.Domain.Interfaces.Repositories.Base
{
    public interface IBaseWriteRepository<T> where T : BaseEntity 
    {
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddAsync(IEnumerable<T> entity, CancellationToken cancellationToken = default);
    }
}
