using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            try
            {
                throw new Exception("This is my error msg!");
            } catch (Exception ex)
            {
                // Proper way to log an error.  Exception along WITH a message.  Never log just the exception
                _logger.LogError(ex, "Something bad happened!");
            }
            
            var post = new BlogPost("This is my test")
            {
                Tags = new[] { "one", "two" }
            };

            // Uses Serilog method to add properties to the context
            using (LogContext.PushProperty("A", 1))
            {
                _logger.LogInformation("Testing the push property");
            }

            // Same as the above example but using the standard ILogger to add properties to the context
            using (_logger.BeginScope("{@CustomId}", 19999))
            {
                _logger.LogInformation("Publishing {@Post}", post);
            }

            int quota = 100;
            string username = "tnatts";

            _logger.LogDebug("Disk quota {Quota} exceeded by user {Username}", quota, username);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
