using AssignmentLayout.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentLayout.Controllers;

public class HomeController : Controller
{
    [HttpGet("/")]
    public ActionResult Index()
    {
        var cities = new List<CityWeather>()
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

        return View(cities);
    }

    [HttpGet("/details/{cityCode:length(3)}")]
    public ActionResult Details(string? cityCode)
    {
        if (cityCode == null)
            return Content("A city code must be provided.");

        var cities = new List<CityWeather>()
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

        var city = cities.Where(c => c.CityUniqueCode == cityCode).FirstOrDefault();

        if (city == null)
            return View("NotFound");

        return View(city);
    }
}
