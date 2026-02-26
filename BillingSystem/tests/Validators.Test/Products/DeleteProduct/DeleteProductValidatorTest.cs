using BillingSystem.Application.Commands.Products.DeleteProduct;
using BillingSystem.Common.Exceptions;
using CommonTestUtilities.Commands.Products.DeleteProduct;

namespace Validators.Test.Products.DeleteProduct
{
    public class DeleteProductValidatorTest
    {
        [Fact]
        public void Success()
        {
            var command = DeleteProductCommandBuilder.Build();
            var validator = new DeleteProductCommandValidator();
            
            var result = validator.Validate(command);
            
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Error_Id_Empty()
        {
            var command = DeleteProductCommandBuilder.Build();
            command = command with { Id = Guid.Empty };
            var validator = new DeleteProductCommandValidator();
            
            var result = validator.Validate(command);
            
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(ResourceMessagesException.ID_EMPTY));
        }
    }
}
