using BillingSystem.Domain.Entities.Customers;
using Bogus;

namespace CommonTestUtilities.Entities
{
    public class CustomerBuilder
    {
        public static Customer Build()
        {
            var faker = new Faker<Customer>()
                .CustomInstantiator(f => new Customer(
                    f.Random.Guid(),
                    f.Person.FullName,
                    f.Internet.Email(),
                    f.Address.FullAddress()
                ));

            return faker.Generate();
        }
    }
}
