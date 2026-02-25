namespace BillingSystem.Domain.Interfaces.Data
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
