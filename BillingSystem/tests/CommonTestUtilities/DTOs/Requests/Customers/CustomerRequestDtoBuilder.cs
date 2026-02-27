using BillingSystem.Application.DTOs.Requests.Customers;
using Bogus;

namespace CommonTestUtilities.DTOs.Requests.Customers
{
    public class CustomerRequestDtoBuilder
    {
        public static IEnumerable<CustomerRequestDto> Build(
            int count = 1,
            int nameLength = 10,
            int emailLength = 10,
            int addressLength = 10
        )
        {
            var customerRequestFaker = new Faker<CustomerRequestDto>()
                .RuleFor(c => c.Name, f => f.Person.FullName.PadRight(nameLength, 'a'))
                .RuleFor(c => c.Email, f => f.Internet.Email(f.Person.FirstName).PadRight(emailLength, 'a'))
                .RuleFor(c => c.Address, f => f.Address.FullAddress().PadRight(addressLength, 'a'));

            return customerRequestFaker.Generate(count);
        }
    }
}
