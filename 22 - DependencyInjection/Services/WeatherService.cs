using ServiceContracts;
using ServiceContracts.Models;

namespace Services;

public class WeatherService : IWeatherService
{
    public List<CityWeather> GetWeatherDetails()
    {
        return new List<CityWeather>()
        {
            new CityWeather
            {
                DateAndTime = Convert.ToDateTime("2030-01-01"),
                TemperatureFahrenheit = 33,
                CityName = "London",
                CityUniqueCode = "LDN",
            },
            new CityWeather
            {
                DateAndTime = Convert.ToDateTime("2030-01-01"),
                TemperatureFahrenheit = 60,
                CityName = "New York City",
                CityUniqueCode = "NYC",
            },
            new CityWeather
            {
                DateAndTime = Convert.ToDateTime("2030-01-01"),
                TemperatureFahrenheit = 82,
                CityName = "Paris",
                CityUniqueCode = "PAR",
            },
        };
    }
}
