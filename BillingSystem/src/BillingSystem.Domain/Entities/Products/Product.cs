using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
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

            Validate();
        }

        public void Update(string name)
        {
            Name = name;

            Validate();
        }

        private void Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(Name))
                errors.Add(ResourceMessagesException.NAME_EMPTY);

            if (errors.Count > 0)
                throw new ErrorOnValidationException(errors);
        }
    }
}
