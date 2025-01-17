using AssignmentPartialViews.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentPartialViews.Controllers;

public class HomeController : Controller
{
    private List<CityWeather> cities = new List<CityWeather>()
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

    [HttpGet("/")]
    public ActionResult Index()
    {
        return View(cities);
    }

    [HttpGet("/details/{cityCode:maxlength(3)}")]
    public ActionResult Details(string cityCode)
    {
        if (cityCode == null)
        {
            return View("NotFound");
        }

        var city = cities.Where(c => c.CityUniqueCode == cityCode).FirstOrDefault();

        if (city == null)
        {
            return View("NotFound");
        }

        return View(city);
    }
}
