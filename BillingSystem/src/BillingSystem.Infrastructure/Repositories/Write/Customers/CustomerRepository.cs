using BillingSystem.Domain.Entities.Customers;
using BillingSystem.Domain.Interfaces.Repositories.Customers;
using BillingSystem.Infrastructure.Data;
using BillingSystem.Infrastructure.Repositories.Write.Base;

namespace BillingSystem.Infrastructure.Repositories.Write.Customers
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerWriteRepository, ICustomerReadRepository
    {
        public CustomerRepository(BillingSystemDbContext context) : base(context) { }
    }
}
