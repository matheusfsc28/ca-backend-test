namespace BillingSystem.Application.DTOs.Responses.Billings
{
    public class ErrorOnSyncBillingResponseDto
    {
        public int SyncedBillingsCount { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public ErrorOnSyncBillingResponseDto(int syncedBillingsCount, IEnumerable<string> errors)
        {
            SyncedBillingsCount = syncedBillingsCount;
            Errors = errors;
        }
    }
}
