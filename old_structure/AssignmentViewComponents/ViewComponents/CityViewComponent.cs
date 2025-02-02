using AssignmentViewComponents.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentViewComponents.ViewComponents;

public class CityViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(CityWeather weather)
    {
        return View(weather);
    }
}
