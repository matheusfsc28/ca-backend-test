using BillingSystem.Application.Commands.Products.UpdateProduct;
using BillingSystem.Common.Exceptions;
using CommonTestUtilities.Commands.Products.UpdateProduct;

namespace Validators.Test.Products.UpdateProduct
{
    public class UpdateProductValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new UpdateProductCommandValidator();
            var request = UpdateProductCommandBuilder.Build();

            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Error_Id_Empty()
        {
            var validator = new UpdateProductCommandValidator();
            var request = UpdateProductCommandBuilder.Build();
            request = request with { Id = Guid.Empty };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceMessagesException.ID_EMPTY));
        }

        [Fact]
        public void Error_Name_Empty()
        {
            var validator = new UpdateProductCommandValidator();
            var request = UpdateProductCommandBuilder.Build();
            request.RequestDto.Name = string.Empty;

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceMessagesException.NAME_EMPTY));
        }

        [Fact]
        public void Error_Name_Max_Length()
        {
            var validator = new UpdateProductCommandValidator();
            var request = UpdateProductCommandBuilder.Build(nameLength: 101);

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(string.Format(ResourceMessagesException.NAME_MAX_LENGTH, 100)));
        }
    }
}
