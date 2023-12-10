using Microsoft.Extensions.Logging;

namespace Web.University
{
    public static class DbLoggerExtensions
    {
        public static ILoggerFactory AddDatabase(this ILoggerFactory factory, LogLevel minLevel)
        {
            factory.AddProvider(new DbLoggerProvider((_, logLevel) => logLevel >= minLevel));
            return factory;
        }
    }
}
