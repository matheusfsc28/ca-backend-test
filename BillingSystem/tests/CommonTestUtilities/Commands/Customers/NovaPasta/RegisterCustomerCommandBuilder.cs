using BillingSystem.Application.Commands.Customers.RegisterCustomer;
using Bogus;

namespace CommonTestUtilities.Commands.Customers.NovaPasta
{
    public class RegisterCustomerCommandBuilder
    {
        public static RegisterCustomerCommand Build(int nameLength = 10)
        {
            return new Faker<RegisterCustomerCommand>()
                .CustomInstantiator(f => new RegisterCustomerCommand(
                    Guid.NewGuid(),
                    f.Person.FullName.PadRight(nameLength, 'a'),
                    f.Internet.Email(f.Person.FirstName).ToLower(),
                    f.Address.FullAddress()
                ));
        }
    }
}
