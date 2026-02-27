using BillingSystem.Application.Commands.Billings.SyncBillings;
using BillingSystem.Application.DTOs.Responses.ExternalBillingService;
using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using CommonTestUtilities.Abstractions.ExternalBillingService;
using CommonTestUtilities.Data.UnitOfWork;
using CommonTestUtilities.DTOs.Responses.ExternalBillingService;
using CommonTestUtilities.Repositories.Billings;
using CommonTestUtilities.Repositories.Customers;
using CommonTestUtilities.Repositories.Products;
using System.Net;

namespace UseCases.Test.Billings.SyncBillings
{
    public class SyncBillingsCommandHandlerTest
    {
        [Fact]
        public async Task Success()
        {
            var numberOfBillings = new Random().Next(1, 11);

            var mockBillings = ExternalBillingResponseDtoBuilder.Build(numberOfBillings);

            var customerIds = mockBillings.Select(b => b.Customer.Id).ToArray();
            var productIds = mockBillings.SelectMany(b => b.Lines).Select(l => l.ProductId).Distinct().ToList();

            var handler = CreateHandler(mockBillings, customerIds, productIds);

            var result = await handler.Handle(new SyncBillingsCommand(), CancellationToken.None);

            Assert.Equal(numberOfBillings, result.SyncedBillingsCount);
        }

        [Fact]
        public async Task Error_Customer_Not_Exists()
        {
            var mockBillings = ExternalBillingResponseDtoBuilder.Build(1);
            var productIds = mockBillings[0].Lines.Select(l => l.ProductId).ToList();

            var handler = CreateHandler(mockBillings, existingCustomerIds: [], existingProductIds: productIds);

            async Task value() => await handler.Handle(new SyncBillingsCommand(), CancellationToken.None);
            Func<Task> act = value;

            var expectedExceptionMessage = string.Format(
                ResourceMessagesException.CUSTOMER_FROM_EXTERNAL_API_NOT_EXISTS,
                mockBillings[0].Customer.Name,
                mockBillings[0].Customer.Id,
                mockBillings[0].InvoiceNumber
            );

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);

            Assert.Contains(exception.GetErrorMessages(), msg => msg.Contains(expectedExceptionMessage));
            Assert.Equal(HttpStatusCode.BadRequest, exception.GetStatusCode());
        }

        [Fact]
        public async Task Error_Product_Not_Exists()
        {
            var mockBillings = ExternalBillingResponseDtoBuilder.Build(1);
            var customerId = mockBillings[0].Customer.Id;

            var handler = CreateHandler(mockBillings, existingCustomerIds: [customerId], existingProductIds: []);

            async Task value() => await handler.Handle(new SyncBillingsCommand(), CancellationToken.None);
            Func<Task> act = value;

            var expectedExceptionMessage = string.Format(
                ResourceMessagesException.PRODUCT_FROM_EXTERNAL_API_NOT_EXISTS,
                mockBillings[0].Lines.Select(l => l.Description).First(),
                mockBillings[0].Lines.Select(l => l.ProductId).First(),
                mockBillings[0].InvoiceNumber
            );

            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(act);
            Assert.Contains(exception.GetErrorMessages(), msg => msg.Contains(expectedExceptionMessage));
            Assert.Equal(HttpStatusCode.BadRequest, exception.GetStatusCode());
        }

        [Fact]
        public async Task Success_When_Invoice_Already_Exists_Should_Ignore()
        {
            var mockBillings = ExternalBillingResponseDtoBuilder.Build(1);
            var invoiceNumber = mockBillings[0].InvoiceNumber;

            var handler = CreateHandler(mockBillings, existingInvoiceNumbers: [invoiceNumber!]);

            var result = await handler.Handle(new SyncBillingsCommand(), CancellationToken.None);

            Assert.Equal(0, result.SyncedBillingsCount);
        }

        [Fact]
        public async Task Error_Partial_Sync_Validation()
        {
            var mockBillings = ExternalBillingResponseDtoBuilder.Build(2);

            var validBilling = mockBillings[0];
            var invalidBilling = mockBillings[1];

            var existingCustomerIds = new List<Guid> { validBilling.Customer.Id };
            var existingProductIds = validBilling.Lines.Select(l => l.ProductId).ToList();

            var handler = CreateHandler(
                mockBillings,
                existingCustomerIds: existingCustomerIds,
                existingProductIds: existingProductIds);

            async Task value() => await handler.Handle(new SyncBillingsCommand(), CancellationToken.None);
            Func<Task> act = value;

            var exception = await Assert.ThrowsAsync<ErrorOnSyncBillingException>(act);

            Assert.Equal(1, exception.GetSuccessesCount());
            Assert.Equal(HttpStatusCode.MultiStatus, exception.GetStatusCode());

            var expectedErrorMessage = string.Format(
                ResourceMessagesException.CUSTOMER_FROM_EXTERNAL_API_NOT_EXISTS,
                invalidBilling.Customer.Name,
                invalidBilling.Customer.Id,
                invalidBilling.InvoiceNumber
            );

            Assert.Contains(exception.GetErrorMessages(), msg => msg.Contains(expectedErrorMessage));
        }

        private static SyncBillingsCommandHandler CreateHandler(
        IEnumerable<ExternalBillingResponseDto> externalBillings,
        IEnumerable<Guid>? existingCustomerIds = null,
        IEnumerable<Guid>? existingProductIds = null,
        IEnumerable<string>? existingInvoiceNumbers = null)
        {
            var externalService = new ExternalBillingServiceBuilder().ReturnsBillings(externalBillings).Build();

            var customerRepo = new CustomerReadRepositoryBuilder()
                .ReturnsExistingIds(existingCustomerIds ?? []).Build();

            var productRepo = new ProductReadRepositoryBuilder()
                .ReturnsExistingIds(existingProductIds ?? []).Build();

            var billingReadRepo = new BillingReadRepositoryBuilder()
                .ReturnsExistingInvoices(existingInvoiceNumbers ?? []).Build();

            var billingWriteRepo = BillingWriteRepositoryBuilder.Build();
            var billingLineWriteRepo = BillingLineWriteRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new SyncBillingsCommandHandler(
                externalService,
                customerRepo,
                productRepo,
                billingReadRepo,
                billingWriteRepo,
                billingLineWriteRepo,
                unitOfWork);
        }
    }
}
