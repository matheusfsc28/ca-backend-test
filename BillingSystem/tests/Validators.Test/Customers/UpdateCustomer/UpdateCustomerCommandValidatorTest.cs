using BillingSystem.Application.Commands.Customers.UpdateCustomer;
using BillingSystem.Common.Exceptions;
using CommonTestUtilities.Commands.Customers.UpdateCustomer;

namespace Validators.Test.Customers.UpdateCustomer
{
    public class UpdateCustomerCommandValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new UpdateCustomerCommandValidator();
            var request = UpdateCustomerCommandBuilder.Build();

            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Success_When_Fields_Are_Null_Should_Ignore_Validation()
        {
            var validator = new UpdateCustomerCommandValidator();
            var request = UpdateCustomerCommandBuilder.Build();

            request = request with
            {
                RequestDto = request.RequestDto with { Name = null, Email = null }
            };

            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Error_Id_Empty()
        {
            var validator = new UpdateCustomerCommandValidator();
            var request = UpdateCustomerCommandBuilder.Build(id: Guid.Empty);

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceMessagesException.ID_EMPTY));
        }

        [Fact]
        public void Error_Name_Empty()
        {
            var validator = new UpdateCustomerCommandValidator();
            var request = UpdateCustomerCommandBuilder.Build();
            request = request with { RequestDto = request.RequestDto with { Name = string.Empty } };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceMessagesException.NAME_EMPTY));
        }

        [Fact]
        public void Error_Name_Max_Length()
        {
            var validator = new UpdateCustomerCommandValidator();
            var request = UpdateCustomerCommandBuilder.Build(nameLength: 151);

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(string.Format(ResourceMessagesException.NAME_MAX_LENGTH, 150)));
        }

        [Fact]
        public void Error_Email_Empty()
        {
            var validator = new UpdateCustomerCommandValidator();
            var request = UpdateCustomerCommandBuilder.Build();
            request = request with { RequestDto = request.RequestDto with { Email = string.Empty } };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceMessagesException.EMAIL_EMPTY));
        }

        [Fact]
        public void Error_Email_Max_Length()
        {
            var validator = new UpdateCustomerCommandValidator();
            var request = UpdateCustomerCommandBuilder.Build(emailLenght: 151);

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(string.Format(ResourceMessagesException.EMAIL_MAX_LENGTH, 150)));
        }

        [Fact]
        public void Error_Address_Empty()
        {
            var validator = new UpdateCustomerCommandValidator();
            var request = UpdateCustomerCommandBuilder.Build();
            request = request with { RequestDto = request.RequestDto with { Address = string.Empty } };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceMessagesException.ADDRESS_EMPTY));
        }

        [Fact]
        public void Error_Address_Max_Length()
        {
            var validator = new UpdateCustomerCommandValidator();
            var request = UpdateCustomerCommandBuilder.Build(addressLength: 251);

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(string.Format(ResourceMessagesException.ADDRESS_MAX_LENGTH, 250)));
        }

    }
}
