using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Domain.Entities.Billings;

namespace BillingSystem.Application.DTOs.Responses.Billings
{
    public class BillingLineResponseDto : BaseResponseDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }

        public BillingLineResponseDto(BillingLine billingLine)
        {
            Id = billingLine.Id;
            ProductId = billingLine.ProductId;
            ProductName = billingLine.Product.Name;
            Quantity = billingLine.Quantity;
            UnitPrice = billingLine.UnitPrice;
            Subtotal = billingLine.Subtotal;
        }
    }
}
