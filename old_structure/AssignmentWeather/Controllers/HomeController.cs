using Microsoft.AspNetCore.Mvc;
using AssignmentWeather.Models;

namespace AssignmentWeather.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        [Route("/home")]
        public ActionResult Index()
        {
            var cities = new List<CityWeather>()
            {
                new CityWeather { DateAndTime = Convert.ToDateTime("2030-01-01"), TemperatureFahrenheit = 33, CityName = "London", CityUniqueCode = "LDN" },
                new CityWeather { DateAndTime = Convert.ToDateTime("2030-01-01"), TemperatureFahrenheit = 60, CityName = "New York City", CityUniqueCode = "NYC" },
                new CityWeather { DateAndTime = Convert.ToDateTime("2030-01-01"), TemperatureFahrenheit = 82, CityName = "Paris", CityUniqueCode = "PAR" }
            };

            return View(cities);
        }

        [Route("/details/{code}")]
        public ActionResult Details(string? code)
        {
            if (code == null)
                return Content("Name has to be supplied.");

            var cities = new List<CityWeather>()
            {
                new CityWeather { DateAndTime = Convert.ToDateTime("2030-01-01"), TemperatureFahrenheit = 33, CityName = "London", CityUniqueCode = "LDN" },
                new CityWeather { DateAndTime = Convert.ToDateTime("2030-01-01"), TemperatureFahrenheit = 60, CityName = "New York City", CityUniqueCode = "NYC" },
                new CityWeather { DateAndTime = Convert.ToDateTime("2030-01-01"), TemperatureFahrenheit = 82, CityName = "Paris", CityUniqueCode = "PAR" }
            };
            var city = cities.Where(c => c.CityUniqueCode == code).FirstOrDefault();

            return View(city);
        }
    }
}
