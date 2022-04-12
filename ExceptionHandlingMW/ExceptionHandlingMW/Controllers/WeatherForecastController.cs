using ExceptionHandling.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionHandlingMW.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
       

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public ActionResult<IEnumerable<WeatherForecast>> Get(string city)
        {
            //try
            //{

            ////if (city == "Sydney")
            ////    throw new Exception("No weather data for Sydney");
            ////return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            ////{
            ////    Date = DateTime.Now.AddDays(index),
            ////    TemperatureC = Random.Shared.Next(-20, 55),
            ////    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            ////})
            ////.ToArray();
            //}
            //catch (Exception e)
            //{

            //    return NotFound(e.Message);
            //}
            return Ok(_weatherService.Get(city));
        }
    }
}