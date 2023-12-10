using Web.University.Domain;
using Microsoft.Extensions.Logging;
using System;

namespace Web.University
{
    public class DbLoggerProvider(Func<string, LogLevel, bool> filter) : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) =>
            new DbLogger<Error>(categoryName, filter);

        public void Dispose()
        {
        }
    }
}
