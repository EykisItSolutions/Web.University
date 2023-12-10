using Web.University.Domain;

namespace Web.University;

public class DbLogger<TLog>(string categoryName, Func<string, LogLevel, bool> filter) : ILogger where TLog : Error, new()
{
    public bool IsEnabled(LogLevel logLevel)
    {
        return filter == null || filter(categoryName, logLevel);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;
        try
        {
            var httpContextAccessor = ServiceLocator.Resolve<IHttpContextAccessor>();
            var httpContext = httpContextAccessor?.HttpContext;
            if (httpContext == null) return;  // Nothing to log

            var error = new Error
            {
                Message = exception?.Message ?? "No message",
                Exception = exception?.ToString(),
                UserAgent = httpContext.Request.Headers["User-Agent"],
                IpAddress = httpContext.Connection?.LocalIpAddress?.ToString(),
                Url = httpContext.Request.Path,
                HttpReferer = httpContext.Request.Headers["Referer"]
            };

            error.CreatedOn = error.ChangedOn = DateTime.Now;
            
            try
            {
                var userId = (httpContext.RequestServices.GetService(typeof(ICurrentUser)) as CurrentUser)?.Id;
                if (userId != 0)
                    error.UserId = error.CreatedBy = error.ChangedBy = userId;
            }
            catch { }

            // Use brand new context: create scope files
            //using var db = new UniversityContext(); ;// connectionString);
            //db.Errors.Add(error);
            //db.SaveChanges();
        }
        catch
        {
            /* possibly notify by email about logging error */
        }
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        throw new NotImplementedException();
    }

}