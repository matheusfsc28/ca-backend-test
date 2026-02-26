using BillingSystem.Application.Commands.Customers.RegisterCustomer;
using BillingSystem.Common.Exceptions;
using CommonTestUtilities.Commands.Customers.NovaPasta;

namespace Validators.Test.Customers.RegisterCustomer
{
    public class RegisterCustomerValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new RegisterCustomerValidator();
            var request = RegisterCustomerCommandBuilder.Build();

            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Error_Name_Empty()
        {
            var validator = new RegisterCustomerValidator();
            var request = RegisterCustomerCommandBuilder.Build();
            request = request with { Name = string.Empty };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceMessagesException.NAME_EMPTY));
        }

        [Fact]
        public void Error_Email_Empty()
        {
            var validator = new RegisterCustomerValidator();
            var request = RegisterCustomerCommandBuilder.Build();
            request = request with { Email = string.Empty };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceMessagesException.EMAIL_EMPTY));
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            var validator = new RegisterCustomerValidator();
            var request = RegisterCustomerCommandBuilder.Build();
            request = request with { Email = "email_invalido.com" };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceMessagesException.EMAIL_INVALID));
        }

        [Fact]
        public void Error_Address_Empty()
        {
            var validator = new RegisterCustomerValidator();
            var request = RegisterCustomerCommandBuilder.Build();
            request = request with { Address = string.Empty };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceMessagesException.ADDRESS_EMPTY));
        }
    }
}
