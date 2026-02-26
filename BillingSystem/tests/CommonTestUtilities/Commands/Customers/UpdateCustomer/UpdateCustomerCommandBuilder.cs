using BillingSystem.Application.Commands.Customers.UpdateCustomer;
using BillingSystem.Application.DTOs.Requests.Customers;
using Bogus;

namespace CommonTestUtilities.Commands.Customers.UpdateCustomer
{
    public class UpdateCustomerCommandBuilder
    {
        public static UpdateCustomerCommand Build(Guid? id = null, int nameLength = 10, int emailLenght = 10, int addressLength = 10)
        {
            return new Faker<UpdateCustomerCommand>()
                .CustomInstantiator(f => new UpdateCustomerCommand(
                    id ?? Guid.NewGuid(),
                    new CustomerRequestDto(
                        f.Person.FullName.PadRight(nameLength, 'a'),
                        f.Internet.Email(f.Person.FirstName).PadRight(emailLenght, 'a').ToLower(),
                        f.Address.FullAddress().PadRight(addressLength, 'a')
                    )
                ));
        }
    }
}
