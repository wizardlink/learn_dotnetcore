using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp;

namespace MyApp.Controllers;

public class HomeController : Controller
{
    private readonly IOptions<SocialMediaLinks> _options;

    public HomeController(IOptions<SocialMediaLinks> options)
    {
        _options = options;
    }

    [Route("/")]
    public ActionResult Index()
    {
        ViewBag.Facebook = _options.Value.Facebook;
        ViewBag.Instagram = _options.Value.Instagram;
        ViewBag.Twitter = _options.Value.Twitter;
        ViewBag.Youtube = _options.Value.Youtube;

        return View();
    }
}
