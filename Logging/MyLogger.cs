using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
//using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using System;
using System.Reflection;
using Serilog.Sinks.Datadog.Logs;

namespace Logging
{
    public class MyLogger
    {
        public static Logger GetInstance()
        {
            var assembly = Assembly.GetEntryAssembly().GetName().Name;
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == Environments.Development;
            var dataDogApiKey = Environment.GetEnvironmentVariable("DatadogApiKey");
            var dataDogLogFilePath = Environment.GetEnvironmentVariable("DataDogLogFilePath");
            var dataDogFileName = (string.IsNullOrEmpty(dataDogLogFilePath) ? "" : $"{dataDogLogFilePath}{assembly}.json");

            // https://docs.datadoghq.com/logs/log_configuration/attributes_naming_convention/
            //var config = new DatadogConfiguration(url: "https://http-intake.logs.us5.datadoghq.com");

            return new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)  // suppress information-level messages from ASP.NET Core components
                .Enrich.FromLogContext()  // allows the use of PushProperty() to dynamically add or remove properties
                .Enrich.WithMachineName()  // https://github.com/serilog/serilog-enrichers-environment
                .Enrich.WithThreadId()   // https://github.com/serilog/serilog-enrichers-thread
                .Enrich.WithThreadName()
                .Enrich.WithAssemblyName()   // https://github.com/TinyBlueRobots/Serilog.Enrichers.AssemblyName
                .Enrich.WithAssemblyVersion()
                .Enrich.WithClientIp()   // https://github.com/mo-esmp/serilog-enrichers-clientinfo
                .Enrich.WithClientAgent()
                .Enrich.WithCorrelationId()   // https://github.com/ekmsystems/serilog-enrichers-correlation-id
                .Enrich.WithProcessId()   // https://github.com/serilog/serilog-enrichers-process
                .Enrich.WithProcessName()
                .MinimumLevel.Debug()
                //.WriteTo.File(@$"c:\LoggingPlaygroundLogs\{assembly}-log.txt", rollingInterval: RollingInterval.Day)
                //.WriteTo.File(new RenderedCompactJsonFormatter(), @$"c:\LoggingPlaygroundLogs\{assembly}-log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Conditional(evt => !string.IsNullOrEmpty(dataDogApiKey), wt =>
                    wt.DatadogLogs(
                        dataDogApiKey ?? "doNotWriteToDataDog",
                        host: System.Environment.MachineName,
                        source: "dotnet",
                        service: assembly,
                        configuration: new DatadogConfiguration(url: "https://http-intake.logs.us5.datadoghq.com")
                    ))
                .WriteTo.Conditional(evt => !string.IsNullOrEmpty(dataDogLogFilePath), wt =>
                    wt.File(new JsonFormatter(renderMessage: true), dataDogFileName))
                .WriteTo.File(new JsonFormatter(), @$"c:\LoggingPlaygroundLogs\{assembly}-log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Conditional(evt => isDevelopment, wt => wt.Seq(Environment.GetEnvironmentVariable("SEQ_URL") ?? "http://localhost:5341"))
                .CreateLogger();
        }
    }
}
