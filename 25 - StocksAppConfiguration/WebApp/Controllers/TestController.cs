using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class TestController : Controller
{
    private readonly IConfigurationRoot root;

    public TestController(IConfiguration configurationRoot)
    {
        root = (IConfigurationRoot)configurationRoot;
    }

    [Route("/test")]
    public IActionResult Index()
    {
        string finalString = string.Empty;
        foreach (var provider in root.Providers.ToList())
        {
            finalString += provider.ToString() + "\n";
        }

        System.Console.Write(finalString);

        return View("Home");
    }
}
