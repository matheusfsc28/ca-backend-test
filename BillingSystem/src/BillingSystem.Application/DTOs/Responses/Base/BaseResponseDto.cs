namespace BillingSystem.Application.DTOs.Responses.Base
{
    public abstract class BaseResponseDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
