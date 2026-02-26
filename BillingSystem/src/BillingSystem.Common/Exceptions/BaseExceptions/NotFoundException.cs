using System.Net;

namespace BillingSystem.Common.Exceptions.BaseExceptions
{
    public class NotFoundException : BillingSystemException
    {
        public NotFoundException(string message) : base(message) { }

        public override IEnumerable<string> GetErrorMessages() => [Message];
        public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
    }
}
