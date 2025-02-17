using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.DTO.Order;
using Models.View.Order;
using Services.Contracts;
using WebApp.Configuration;

namespace WebApp.Controllers;

public class TradeController : Controller
{
    private readonly IOptions<TradingOptions> _options;
    private readonly IConfiguration _configuration;
    private readonly IFinnhubService _finnhubService;
    private readonly IStocksService _stocksService;

    public TradeController(
        IOptions<TradingOptions> options,
        IConfiguration configuration,
        IFinnhubService finnhubService,
        IStocksService stocksService
    )
    {
        _options = options;
        _configuration = configuration;
        _finnhubService = finnhubService;
        _stocksService = stocksService;
    }

    [Route("/{symbol:length(4)?}")]
    public async Task<ActionResult> Index(string symbol)
    {
        string stockSymbol = _options.Value.DefaultStockSymbol ?? "GOOG";

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

    [Route("/orders")]
    public ActionResult Orders()
    {
        Orders orders = new()
        {
            BuyResponses = _stocksService.GetBuyOrders(),
            SellResponses = _stocksService.GetSellOrders(),
        };

        return View(orders);
    }

    [Route("/buyorder")]
    public ActionResult BuyOrder(
        [FromQuery] string quantity,
        [FromQuery] string price,
        [FromQuery] string stockSymbol,
        [FromQuery] string stockName
    )
    {
        BuyRequest request = new()
        {
            Price = double.Parse(price),
            Quantity = uint.Parse(quantity),
            StockName = stockName,
            StockSymbol = stockSymbol,
            DateAndTimeOfOrder = DateTime.Now,
        };
        _stocksService.CreateBuyOrder(request);

        return RedirectToAction("Orders", "Trade");
    }

    [Route("/sellorder")]
    public ActionResult SellOrder(
        [FromQuery] string quantity,
        [FromQuery] string price,
        [FromQuery] string stockSymbol,
        [FromQuery] string stockName
    )
    {
        SellRequest request = new()
        {
            Price = double.Parse(price),
            Quantity = uint.Parse(quantity),
            StockName = stockName,
            StockSymbol = stockSymbol,
            DateAndTimeOfOrder = DateTime.Now,
        };
        _stocksService.CreateSellOrder(request);

        return RedirectToAction("Orders", "Trade");
    }
}
