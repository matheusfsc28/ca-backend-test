using BillingSystem.Application.DTOs.Responses.ExternalBillingService;
using Bogus;

namespace CommonTestUtilities.DTOs.Responses.ExternalBillingService
{
    public class ExternalBillingResponseDtoBuilder
    {
        public static List<ExternalBillingResponseDto> Build(int count = 1)
        {
            var customerFaker = new Faker<ExternalCustomerResponseDto>()
                .RuleFor(c => c.Id, f => f.Random.Guid())
                .RuleFor(c => c.Name, f => f.Person.FullName)
                .RuleFor(c => c.Address,  f => f.Address.FullAddress());

            var lineFaker = new Faker<ExternalBillingLineResponseDto>()
                .RuleFor(l => l.ProductId, f => f.Random.Guid())
                .RuleFor(l => l.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(l => l.UnitPrice, f => f.Finance.Amount(5, 100));

            var billingFaker = new Faker<ExternalBillingResponseDto>()
                .RuleFor(b => b.InvoiceNumber, f => f.Commerce.Ean13())
                .RuleFor(b => b.Date, f => f.Date.Recent())
                .RuleFor(b => b.DueDate, f => f.Date.Future())
                .RuleFor(b => b.TotalAmount, f => f.Finance.Amount(100, 1000))
                .RuleFor(b => b.Currency, "BRL")
                .RuleFor(b => b.Customer, f => customerFaker.Generate())
                .RuleFor(b => b.Lines, f => lineFaker.Generate(2));

            return billingFaker.Generate(count);
        }
    }
}
