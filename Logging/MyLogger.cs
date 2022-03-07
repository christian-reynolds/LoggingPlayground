﻿using Serilog;
using Serilog.Core;
using System.Reflection;

namespace Logging
{
    public class MyLogger
    {
        public static Logger GetInstance()
        {
            string assembly = Assembly.GetEntryAssembly().GetName().Name;

            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.File(@$"c:\LoggingPlaygroundLogs\{assembly}-log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
