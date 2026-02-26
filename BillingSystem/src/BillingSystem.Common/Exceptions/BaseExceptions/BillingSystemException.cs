using System.Net;

namespace BillingSystem.Common.Exceptions.BaseExceptions
{
    public abstract class BillingSystemException : Exception
    {
        protected BillingSystemException(string message) : base(message) { }
        public abstract IEnumerable<string> GetErrorMessages();
        public abstract HttpStatusCode GetStatusCode();
    }
}
