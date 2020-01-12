using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace sample_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly Gauge weatherForecastsInProgress = Metrics
            .CreateGauge("myapp_weather_forecasts_in_progress", 
                         "Number of weather forecast operations ongoing.");
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private Random rnd = new Random((int)DateTime.Now.Ticks);

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            using(weatherForecastsInProgress.TrackInProgress())
            {
                Thread.Sleep(rnd.Next(2000));
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rnd.Next(-20, 55),
                    Summary = Summaries[rnd.Next(Summaries.Length)]
                })
                .ToArray();
            }
        }
    }
}
