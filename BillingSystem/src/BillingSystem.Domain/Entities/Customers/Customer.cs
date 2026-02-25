using BillingSystem.Domain.Entities.Base;

namespace BillingSystem.Domain.Entities.Customers
{
    public class Customer : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;

        protected Customer() { }

        public Customer(Guid id, string name, string email, string address)
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
        }

        public void Update(string name, string email, string address)
        {
            Name = name;
            Email = email;
            Address = address;
        }
    }
}
