using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Domain.Entities.Customers;

namespace BillingSystem.Application.DTOs.Responses.Customers
{
    public class CustomerResponseDto : BaseResponseDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public CustomerResponseDto(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Email = customer.Email;
            Address = customer.Address;
            CreatedAt = customer.CreatedAt;
            UpdatedAt = customer.UpdatedAt;
            DeletedAt = customer.DeletedAt;
        }
    }
}
