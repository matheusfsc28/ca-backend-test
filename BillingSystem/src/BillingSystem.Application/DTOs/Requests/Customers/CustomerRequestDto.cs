namespace BillingSystem.Application.DTOs.Requests.Customers
{
    public record CustomerRequestDto(
        string? Name,
        string? Email,
        string? Address
    );
}
