using BillingSystem.Application.Commands.Products.RegisterProduct;
using BillingSystem.Common.Exceptions;
using CommonTestUtilities.Commands.Products.RegisterProduct;

namespace Validators.Test.Products.RegisterProduct
{
    public class RegisterProductValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new RegisterProductCommandValidator();
            var request = RegisterProductCommandBuilder.Build();
            var result = validator.Validate(request);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Error_Id_Empty()
        {
            var validator = new RegisterProductCommandValidator();
            var request = RegisterProductCommandBuilder.Build();
            request = request with { Id = Guid.Empty };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceMessagesException.ID_EMPTY));
        }

        [Fact]
        public void Error_Name_Empty()
        {
            var validator = new RegisterProductCommandValidator();
            var request = RegisterProductCommandBuilder.Build();
            request = request with { Name = string.Empty };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceMessagesException.NAME_EMPTY));
        }

        [Fact]
        public void Error_Max_Name_Length()
        {
            var validator = new RegisterProductCommandValidator();
            var request = RegisterProductCommandBuilder.Build(101);
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(string.Format(ResourceMessagesException.NAME_MAX_LENGTH, 100)));
        }
    }
}
