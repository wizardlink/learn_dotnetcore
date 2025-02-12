using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.View.Order;
using Services.Contracts;
using WebApp.Configuration;

namespace WebApp.Controllers;

public class TradeController : Controller
{
    private readonly IOptions<TradingOptions> _options;
    private readonly IConfiguration _configuration;
    private readonly IFinnhubService _finnhubService;

    public TradeController(
        IOptions<TradingOptions> options,
        IConfiguration configuration,
        IFinnhubService finnhubService
    )
    {
        _options = options;
        _configuration = configuration;
        _finnhubService = finnhubService;
    }

    [Route("/{symbol?}")]
    public async Task<ActionResult> Index(string symbol)
    {
        string stockSymbol = _options.Value.DefaultStockSymbol ?? "MSFT";

        if (symbol != null && symbol != string.Empty)
            stockSymbol = symbol;

        var companyProfile = await _finnhubService.GetCompanyProfile(stockSymbol);
        var stockPrice = await _finnhubService.GetStockPriceQuote(stockSymbol);

        if (companyProfile == null || stockPrice == null)
            return StatusCode(500);

        ViewBag.FinnhubToken = _configuration["FinnhubToken"];

        return View(
            new StockTrade
            {
                Price = stockPrice.CurrentPrice ?? 0,
                StockName = companyProfile.Name,
                StockSymbol = stockSymbol,
            }
        );
    }
}
