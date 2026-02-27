using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Application.DTOs.Responses.Billings;
using BillingSystem.Common.Exceptions;
using BillingSystem.Common.Exceptions.BaseExceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BillingSystem.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ErrorOnSyncBillingException errorOnSyncBillingException)
                HandleErrorOnSyncBillingValidationException(errorOnSyncBillingException, context);
            else if (context.Exception is BillingSystemException billingSystemException)
                HandleProjectException(billingSystemException, context);
            else
                HandleUnknownException(context);
        }

        private void HandleErrorOnSyncBillingValidationException(ErrorOnSyncBillingException exception, ExceptionContext context)
        {
            var statusCode = (int)exception.GetStatusCode();
            var errors = exception.GetErrorMessages();
            var successCount = exception.GetSuccessesCount();

            _logger.LogWarning(
                    context.Exception,
                    "Invoice synchronization completed with alerts. Successes: {SuccessCount}. Failures: {ErrorCount}. Status: {StatusCode}. Errors: {@Errors}",
                    successCount,
                    errors.Count(),
                    statusCode,
                    errors);

            context.ExceptionHandled = true;

            context.HttpContext.Response.StatusCode = statusCode;
            context.Result = new ObjectResult(new ErrorOnSyncBillingResponseDto(successCount, errors));
        }

        private void HandleProjectException(BillingSystemException billingSystemException, ExceptionContext context)
        {
            var statusCode = (int)billingSystemException.GetStatusCode();
            var errors = billingSystemException.GetErrorMessages();

            _logger.LogWarning(
                context.Exception,
                "Handled business exception. Status: {StatusCode}. Errors: {@Errors}",
                statusCode,
                errors);

            context.ExceptionHandled = true;

            context.HttpContext.Response.StatusCode = statusCode;
            context.Result = new ObjectResult(new ErrorResponseDto(errors));
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            _logger.LogError(
                context.Exception,
                "Unhandled exception caught in Controller: {ControllerName}, Action: {ActionName}, Error: {Message}",
                context.ActionDescriptor.DisplayName,
                context.HttpContext.Request.Method,
                context.Exception.Message);

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ErrorResponseDto(ResourceMessagesException.UNKNOWN_ERROR));
        }
    }
}
