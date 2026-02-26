using BillingSystem.Domain.Entities.Customers;
using BillingSystem.Domain.Interfaces.Repositories.Customers;
using BillingSystem.Infrastructure.Data;
using BillingSystem.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace BillingSystem.Infrastructure.Repositories.Customers
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerWriteRepository, ICustomerReadRepository
    {
        public CustomerRepository(BillingSystemDbContext context) : base(context) { }

        public async Task<bool> EmailRegistered(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(c => c.Email == email, cancellationToken);
        }
    }
}
