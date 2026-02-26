using BillingSystem.Domain.Entities.Customers;
using BillingSystem.Domain.Interfaces.Repositories.Base;

namespace BillingSystem.Domain.Interfaces.Repositories.Customers
{
    public interface ICustomerReadRepository : IBaseReadRepository<Customer>
    {
        Task<bool> EmailRegistered(string email, CancellationToken cancellationToken = default);
        Task<bool> EmailRegistered(Guid id, string email, CancellationToken cancellationToken = default);
    }
}
