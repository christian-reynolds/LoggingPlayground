using Serilog;
using Serilog.Core;
using Serilog.Events;
//using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using System;
using System.Reflection;

namespace Logging
{
    public class MyLogger
    {
        public static Logger GetInstance()
        {
            string assembly = Assembly.GetEntryAssembly().GetName().Name;

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
                .MinimumLevel.Debug()
                //.WriteTo.File(@$"c:\LoggingPlaygroundLogs\{assembly}-log.txt", rollingInterval: RollingInterval.Day)
                //.WriteTo.File(new RenderedCompactJsonFormatter(), @$"c:\LoggingPlaygroundLogs\{assembly}-log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.File(new JsonFormatter(), @$"c:\LoggingPlaygroundLogs\{assembly}-log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Seq(Environment.GetEnvironmentVariable("SEQ_URL") ?? "http://localhost:5341")
                .CreateLogger();
        }
    }
}
