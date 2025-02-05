using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private IWeatherService _weatherService;

        public HomeController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("/")]
        public ActionResult Index()
        {
            return View(_weatherService.GetWeatherDetails());
        }

        [HttpGet("/details/{cityCode:maxlength(3)}")]
        public ActionResult Details(string cityCode)
        {
            if (cityCode == null)
            {
                return View("NotFound");
            }

            var city = _weatherService.GetWeatherDetails().Where(c => c.CityUniqueCode == cityCode).FirstOrDefault();

            if (city == null)
            {
                return View("NotFound");
            }

            return View(city);
        }
    }
}
