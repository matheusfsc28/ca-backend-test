using System.Net;

namespace BillingSystem.Common.Exceptions.BaseExceptions
{
    public class ExternalServiceException : BillingSystemException
    {
        public ExternalServiceException(string message) : base(message) { }

        public override IEnumerable<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.FailedDependency;
    }
}
