using System.Net;

namespace BillingSystem.Common.Exceptions.BaseExceptions
{
    public class ErrorOnValidationException : BillingSystemException
    {
        private readonly IEnumerable<string> _errors;

        public ErrorOnValidationException(IEnumerable<string> errors) : base(string.Empty) => _errors = errors;

        public ErrorOnValidationException(string error) : base(string.Empty) => _errors = [error];

        public override IEnumerable<string> GetErrorMessages() => _errors;

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
    }
}
