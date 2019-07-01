using EmailApi.CustomExceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EmailApi.Filter
{
    public class GeneralExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GeneralExceptionFilter> _logger;

        public GeneralExceptionFilter(ILogger<GeneralExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
             string traceId = context.HttpContext.TraceIdentifier;

            Exception ex = context.Exception;
            HttpStatusCode httpStatusCode;
            string message = context.Exception.Message;

            if (ex is NotFoundException)
            {
                _logger.LogWarning(ex, $"TraceId: {traceId} - {message}", null);
                httpStatusCode = HttpStatusCode.NotFound;
            }
            else if (ex is ValidationException)
            {
                _logger.LogWarning(ex, $"TraceId: {traceId} - {message}", null);
                httpStatusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                _logger.LogError(ex, $"TraceId: {traceId} - {message}", null);
                httpStatusCode = HttpStatusCode.InternalServerError;
                message = "Unexpected error occurs";
            }

            context.Result = new ObjectResult(message)
            {
                StatusCode = (int)httpStatusCode
            };
        }
    }
}