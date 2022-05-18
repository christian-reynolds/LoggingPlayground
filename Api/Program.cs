using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = Logging.MyLogger.GetInstance();
            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            try
            {
                //throw new Exception("Something went wrong!!!");

                Log.Information("Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
