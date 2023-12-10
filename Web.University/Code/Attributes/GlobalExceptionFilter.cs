using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.University;

// Captures unhandled exceptions in the application

public class GlobalExceptionFilter(ILoggerFactory loggerFactory) : IExceptionFilter
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<GlobalExceptionFilter>();

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(0, context.Exception, "Global Exception");
    }
}
