using Microsoft.AspNetCore.Mvc;
using ServiceContracts.Models;

namespace WebApp.ViewComponents;

public class CityViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(CityWeather weather)
    {
        return View(weather);
    }
}
