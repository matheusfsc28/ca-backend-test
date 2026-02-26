using BillingSystem.Application.Commands.Customers.DeleteCustomer;
using Bogus;

namespace CommonTestUtilities.Commands.Customers.DeleteCustomer
{
    public class DeleteCustomerCommandBuilder
    {
        public static DeleteCustomerCommand Build(Guid? id = null)
        {
            return new Faker<DeleteCustomerCommand>()
                .CustomInstantiator(f => new DeleteCustomerCommand(
                    id ?? Guid.NewGuid()
                ));
        }
    }
}
