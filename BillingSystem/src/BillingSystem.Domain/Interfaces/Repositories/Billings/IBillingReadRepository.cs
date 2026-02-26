using BillingSystem.Domain.Entities.Billings;
using BillingSystem.Domain.Interfaces.Repositories.Base;

namespace BillingSystem.Domain.Interfaces.Repositories.Billings
{
    public interface IBillingReadRepository : IBaseReadRepository<Billing>
    {
        Task<HashSet<string>> GetExistingInvoiceNumbersAsync(IEnumerable<string> InvoiceNumber, CancellationToken cancellationToken);
    }
}
