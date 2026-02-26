namespace BillingSystem.Application.DTOs.Requests.Customer
{
    public record CustomerRequestDto(
        string? Name,
        string? Email,
        string? Address
    );
}
