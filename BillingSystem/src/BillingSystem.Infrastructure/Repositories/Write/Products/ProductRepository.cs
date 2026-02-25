using BillingSystem.Domain.Entities.Products;
using BillingSystem.Domain.Interfaces.Repositories.Products;
using BillingSystem.Infrastructure.Data;
using BillingSystem.Infrastructure.Repositories.Write.Base;

namespace BillingSystem.Infrastructure.Repositories.Write.Products
{
    public class ProductRepository : BaseRepository<Product>, IProductWriteRepository, IProductReadRepository
    {
        public ProductRepository(BillingSystemDbContext context) : base(context) { }
    }
}
