using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly IFinnhubService _finnhubService;
    private readonly IOptions<TradingOptions> _options;
    private readonly IConfiguration _configuration;

    public HomeController(
        IFinnhubService finnhubService,
        IOptions<TradingOptions> options,
        IConfiguration configuration
    )
    {
        _finnhubService = finnhubService;
        _options = options;
        _configuration = configuration;
    }

    [Route("/{symbol?}")]
    public async Task<IActionResult> Index(string? symbol)
    {
        if (symbol == null)
            symbol = _options.Value.DefaultStockSymbol!;

        StockPriceQuote priceQuote = await _finnhubService.GetStockPriceQuote(symbol);
        CompanyProfile companyProfile = await _finnhubService.GetCompanyProfile(symbol);

        ViewBag.FinnhubToken = _configuration["FinnhubToken"];

        StockTrade result = new StockTrade
        {
            StockSymbol = symbol,
            StockName = companyProfile.Name,
            Price = priceQuote.CurrentPrice ?? 0,
        };

        return View(result);
    }
}
