using System.Net;

namespace BillingSystem.Common.Exceptions.BaseExceptions
{
    public class ErrorOnSyncBillingException : BillingSystemException
    {
        private readonly IEnumerable<string> _errors;
        private readonly int _successesCount;

        public ErrorOnSyncBillingException(IEnumerable<string> errors, int successesCount) : base(string.Empty)
        {
            _errors = errors;
            _successesCount = successesCount;
        }

        public override IEnumerable<string> GetErrorMessages() => _errors;

        public int GetSuccessesCount() => _successesCount;

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.MultiStatus;
    }
}
