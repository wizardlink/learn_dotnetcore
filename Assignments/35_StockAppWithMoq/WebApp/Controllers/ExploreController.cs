using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace WebApp.Controllers;

public class ExploreController(IFinnhubService finnhubService) : Controller
{
    [HttpGet]
    [Route("[controller]/{selectedSymbol:length(4)?}")]
    public async Task<ActionResult> Index(string selectedSymbol, [FromQuery] bool showAll)
    {
        var stocks = await finnhubService.GetStocks();

        if (stocks is null)
            return StatusCode(404);

        if (!showAll && stocks.Count > 10)
        {
            stocks = stocks[..10];
        }

        if (!string.IsNullOrEmpty(selectedSymbol))
        {
            ViewBag.SelectedCompany = await finnhubService.GetCompanyProfile(selectedSymbol);
            ViewBag.SelectedPrice = await finnhubService.GetStockPriceQuote(selectedSymbol);
            ViewBag.SelectedSymbol = selectedSymbol;
        }

        ViewBag.ShowAll = showAll;

        return View(stocks);
    }
}
