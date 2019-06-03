using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetLoggerPlayground
{
    public class Foo
    {
        private readonly ILogger _logger;

        public Foo(ILogger<Foo> logger)
        {
            _logger = logger;
            _logger.LogDebug("Foo Created");
        }

        public void Exec()
        {
            _logger.LogInformation("Executing");
        }
    }
}
