using BillingSystem.Application.Commands.Customers.DeleteCustomer;
using BillingSystem.Common.Exceptions;
using CommonTestUtilities.Commands.Customers.DeleteCustomer;

namespace Validators.Test.Customers.DeleteCustomer
{
    public class DeleteCustomerValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new DeleteCustomerCommandValidator();
            var request = DeleteCustomerCommandBuilder.Build();

            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Error_Id_Empty()
        {
            var validator = new DeleteCustomerCommandValidator();
            var request = DeleteCustomerCommandBuilder.Build();
            request = request with { Id = Guid.Empty };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceMessagesException.ID_EMPTY));
        }
    }
}
