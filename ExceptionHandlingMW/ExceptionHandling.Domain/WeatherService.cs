using ExceptionHandling.Domain.Exceptions;

namespace ExceptionHandling.Domain
{
    public interface IWeatherService
    { 
        IEnumerable<WeatherForecast> Get(string cityName);
    }
    public class WeatherService : IWeatherService
    {
        private static readonly string[] Summaries = new[]
       {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        public IEnumerable<WeatherForecast> Get(string cityName)
        {
            if (cityName == "Sydney")
            {
                //throw new Exception("No weather data for Sydney");
                throw new DomainValidationException("No weather data for Sydney");
            }

            if (cityName == "Test")
            {
                throw new DomainNotFoundException($"No City found - {cityName}");
            }

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}