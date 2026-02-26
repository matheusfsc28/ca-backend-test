using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
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

            Validate();
        }

        public void Update(string name, string email, string address)
        {
            Name = name;
            Email = email;
            Address = address;

            Validate();
        }

        private void Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(Name))
                errors.Add(ResourceMessagesException.NAME_EMPTY);

            if (Name.Length > 150)
                errors.Add(string.Format(ResourceMessagesException.NAME_MAX_LENGTH, 150));

            if (string.IsNullOrEmpty(Email))
                errors.Add(ResourceMessagesException.EMAIL_EMPTY);

            if (Email.Length > 150)
                errors.Add(string.Format(ResourceMessagesException.EMAIL_MAX_LENGTH, 150));

            if (string.IsNullOrEmpty(Address))
                errors.Add(ResourceMessagesException.ADDRESS_EMPTY);

            if(Address.Length > 250)
                errors.Add(string.Format(ResourceMessagesException.ADDRESS_MAX_LENGTH, 250));

            if (errors.Count > 0)
                throw new ErrorOnValidationException(errors);
        }
    }
}
