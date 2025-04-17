using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.DTO.Order;
using Models.View.Order;
using Services.Contracts;
using WebApp.Configuration;

namespace WebApp.Controllers;

public class TradeController(
    IOptions<TradingOptions> options,
    IConfiguration configuration,
    IFinnhubService finnhubService,
    IStocksService stocksService,
    Serilog.ILogger logger
) : Controller
{
    [Route("/{symbol:length(4)?}")]
    public async Task<ActionResult> Index(string symbol)
    {
        logger.Information("Entered TradeController's Index.");

        string stockSymbol = options.Value.DefaultStockSymbol ?? "GOOG";

        if (symbol != null && symbol != string.Empty)
            stockSymbol = symbol;

        var companyProfile = await finnhubService.GetCompanyProfile(stockSymbol);
        var stockPrice = await finnhubService.GetStockPriceQuote(stockSymbol);

        if (companyProfile == null || stockPrice == null)
            return StatusCode(500);

        ViewBag.FinnhubToken = configuration["FinnhubToken"];

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
        logger.Information("Entered TradeController's Orders.");

        Orders orders = new()
        {
            BuyResponses = stocksService.GetBuyOrders(),
            SellResponses = stocksService.GetSellOrders(),
        };

        return View(orders);
    }

    [Route("/buyorder")]
    public async Task<ActionResult> BuyOrder(
        [FromQuery] string quantity,
        [FromQuery] string price,
        [FromQuery] string stockSymbol,
        [FromQuery] string stockName
    )
    {
        logger.Information("Entered TradeController's BuyOrder.");

        BuyRequest request = new()
        {
            Price = double.Parse(price),
            Quantity = uint.Parse(quantity),
            StockName = stockName,
            StockSymbol = stockSymbol,
            DateAndTimeOfOrder = DateTime.Now,
        };
        await stocksService.CreateBuyOrder(request);

        return RedirectToAction("Orders", "Trade");
    }

    [Route("/sellorder")]
    public async Task<ActionResult> SellOrder(
        [FromQuery] string quantity,
        [FromQuery] string price,
        [FromQuery] string stockSymbol,
        [FromQuery] string stockName
    )
    {
        logger.Information("Entered TradeController's SellOrder.");

        SellRequest request = new()
        {
            Price = double.Parse(price),
            Quantity = uint.Parse(quantity),
            StockName = stockName,
            StockSymbol = stockSymbol,
            DateAndTimeOfOrder = DateTime.Now,
        };
        await stocksService.CreateSellOrder(request);

        return RedirectToAction("Orders", "Trade");
    }
}
