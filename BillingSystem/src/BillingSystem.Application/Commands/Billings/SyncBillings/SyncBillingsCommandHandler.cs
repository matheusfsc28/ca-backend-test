using BillingSystem.Application.Abstractions.ExternalBillingService;
using BillingSystem.Application.DTOs.Responses.Billings;
using BillingSystem.Application.DTOs.Responses.ExternalBillingService;
using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using BillingSystem.Domain.Entities.Billings;
using BillingSystem.Domain.Interfaces.Data;
using BillingSystem.Domain.Interfaces.Repositories.Billings;
using BillingSystem.Domain.Interfaces.Repositories.Customers;
using BillingSystem.Domain.Interfaces.Repositories.Products;
using MediatR;

namespace BillingSystem.Application.Commands.Billings.SyncBillings
{
    public class SyncBillingsCommandHandler : IRequestHandler<SyncBillingsCommand, SyncBillingsResponseDto>
    {
        private readonly IExternalBillingService _externalService;
        private readonly ICustomerReadRepository _customerReadRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IBillingReadRepository _billingReadRepository;
        private readonly IBillingWriteRepository _billingWriteRepository;
        private readonly IBillingLineWriteRepository _billingLineWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SyncBillingsCommandHandler(
            IExternalBillingService externalService,
            ICustomerReadRepository customerReadRepository,
            IProductReadRepository productReadRepository,
            IBillingReadRepository billingReadRepository,
            IBillingWriteRepository billingWriteRepository,
            IBillingLineWriteRepository billingLineWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _externalService = externalService;
            _customerReadRepository = customerReadRepository;
            _productReadRepository = productReadRepository;
            _billingReadRepository = billingReadRepository;
            _billingWriteRepository = billingWriteRepository;
            _billingLineWriteRepository = billingLineWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SyncBillingsResponseDto> Handle(SyncBillingsCommand request, CancellationToken cancellationToken)
        {
            var externalBillings = await _externalService.GetBillingsAsync(cancellationToken);
            var billingsToProcess = await FilterPendingBillingsToSyncAsync(externalBillings, cancellationToken);

            if (!billingsToProcess.Any())
                return new SyncBillingsResponseDto();

            var customerIdsToSync = billingsToProcess.Select(b => b!.Customer.Id).Distinct();
            var productIdsToSync = billingsToProcess.SelectMany(b => b!.Lines.Select(l => l.ProductId)).Distinct();

            var existingCustomerIds = await _customerReadRepository.GetExistingIdsAsync(customerIdsToSync, cancellationToken);
            var existingProductIds = await _productReadRepository.GetExistingIdsAsync(productIdsToSync, cancellationToken);

            var billingsToCreate = new List<Billing>();
            var billingLinesToCreate = new List<BillingLine>();
            var validationErrors = new List<string>();

            foreach (var billingDto in billingsToProcess)
            {
                ProcessBillingDto(
                    billingDto!,
                    existingCustomerIds,
                    existingProductIds,
                    billingsToCreate,
                    billingLinesToCreate,
                    validationErrors);
            }

            if (billingsToCreate.Count != 0)
            {
                await _billingWriteRepository.AddAsync(billingsToCreate, cancellationToken);

                if (billingLinesToCreate.Count != 0)
                    await _billingLineWriteRepository.AddAsync(billingLinesToCreate, cancellationToken);

                await _unitOfWork.CommitAsync(cancellationToken);
            }

            if (validationErrors.Count != 0)
            {
                if (billingsToCreate.Count == 0)
                    throw new ErrorOnValidationException(validationErrors);
                else
                    throw new ErrorOnSyncBillingValidation(validationErrors, billingsToCreate.Count);
            }

            return new SyncBillingsResponseDto(billingsToCreate.Count);
        }

        private async Task<IEnumerable<ExternalBillingResponseDto>> FilterPendingBillingsToSyncAsync(
            IEnumerable<ExternalBillingResponseDto?> externalBillings,
            CancellationToken cancellationToken)
        {
            var validBillings = externalBillings
                .Where(b => b != null && b.IsValidForSync)
                .GroupBy(b => b!.InvoiceNumber)
                .Select(bg => bg.OrderByDescending(b => b!.Date).FirstOrDefault()!)
                .ToList();

            if (validBillings.Count == 0)
                return [];

            var invoiceNumbersToSync = validBillings.Select(b => b.InvoiceNumber!).Distinct();
            var existingInvoiceNumbers = await _billingReadRepository.GetExistingInvoiceNumbersAsync(invoiceNumbersToSync, cancellationToken);
            var existingInvoicesSet = existingInvoiceNumbers.ToHashSet();

            return validBillings
                .Where(b => !existingInvoicesSet.Contains(b.InvoiceNumber!));
        }

        private static void ProcessBillingDto(
            ExternalBillingResponseDto billingDto,
            HashSet<Guid> existingCustomerIds,
            HashSet<Guid> existingProductIds,
            List<Billing> billingsToCreate,
            List<BillingLine> billingLinesToCreate,
            List<string> validationErrors)
        {
            var customerExists = existingCustomerIds.Contains(billingDto.Customer.Id);

            if (!customerExists)
                validationErrors.Add(
                    string.Format(
                        ResourceMessagesException.CUSTOMER_FROM_EXTERNAL_API_NOT_EXISTS, 
                        billingDto.Customer.Name, 
                        billingDto.Customer.Id, 
                        billingDto.InvoiceNumber
                    )
                );

            var billing = new Billing(
                billingDto.InvoiceNumber,
                billingDto.Customer.Id,
                billingDto.Date,
                billingDto.DueDate,
                billingDto.TotalAmount,
                billingDto.Currency
            );

            var tempLines = new List<BillingLine>();
            var hasAnyProductMissing = false;

            foreach (var extLine in billingDto.Lines)
            {
                if (!existingProductIds.Contains(extLine.ProductId))
                {
                    validationErrors.Add(
                        string.Format(
                            ResourceMessagesException.PRODUCT_FROM_EXTERNAL_API_NOT_EXISTS, 
                            extLine.Description, 
                            extLine.ProductId, 
                            billingDto.InvoiceNumber
                        )
                    );
                    hasAnyProductMissing = true;
                }
                else if (customerExists && !hasAnyProductMissing)
                {
                    tempLines.Add(new BillingLine(billing.Id, extLine.ProductId, extLine.Quantity, extLine.UnitPrice));
                }
            }

            if (customerExists && !hasAnyProductMissing)
            {
                billingsToCreate.Add(billing);
                billingLinesToCreate.AddRange(tempLines);
            }
        }
    }
}