using BillingSystem.Domain.Entities.Base;

namespace BillingSystem.Domain.Entities.Products
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;

        protected Product() { }

        public Product(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public void Update(string name)
        {
            Name = name;
        }
    }
}
