using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models.View.Home;

namespace WebApp.Controllers;

[Route("[controller]")]
public class HomeController : Controller
{
    [HttpGet("Error")]
    public ActionResult Error()
    {
        ErrorDetails details = new()
        {
            Message = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error.Message ?? string.Empty,
        };

        return View(details);
    }
}
