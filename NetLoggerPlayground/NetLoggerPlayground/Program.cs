using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
// credit: https://www.blinkingcaret.com/2018/02/14/net-core-console-logging/

namespace NetLoggerPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var foo = serviceProvider.GetService<Foo>();

            foo.Exec();

            Console.ReadKey();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // SeriLog
            Log.Logger = new LoggerConfiguration()
                    //.Filter.ByIncludingOnly(l => l.Properties.ContainsKey("Some tag"))
                    .WriteTo.File("main.log")
                    .CreateLogger();

            // General
            IConfiguration configuration = new ConfigurationBuilder()
                                       .AddEnvironmentVariables()
                                       .Build();

            services
                    .Configure<LoggerFilterOptions>(options =>
                    {
                        options.AddFilter((category, level) => true);
                        options.Rules.Add(new LoggerFilterRule("Console", "NetLoggerPlayground.Foo", LogLevel.Trace, (provider, category, level) => true));
                        if (Enum.TryParse(configuration["LOG_LEVEL"], out LogLevel logLevel))
                            options.MinLevel = logLevel;
                        else
                            options.MinLevel = LogLevel.Information;
                    })
                    .AddLogging(configure =>
                            configure.AddConsole())
                    .AddSingleton<Foo>();
            services.AddLogging(configure => configure.AddSerilog());
        }
    }
}
