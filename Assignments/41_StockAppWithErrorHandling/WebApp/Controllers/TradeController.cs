using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.DTO.Order;
using Models.Entities.Order;
using Models.View.Order;
using Services.Contracts;
using WebApp.Configuration;
using WebApp.Filters;

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
            new BaseOrder
            {
                Price = stockPrice.CurrentPrice ?? 0,
                StockName = companyProfile.Name ?? string.Empty,
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
    [TypeFilter<CreateOrderActionFilter>()]
    public async Task<ActionResult> BuyOrder([FromForm] BaseOrder incomingOrder)
    {
        logger.Information("Entered TradeController's BuyOrder.");

        await stocksService.CreateBuyOrder(incomingOrder.ToBuyRequest());

        return RedirectToAction("Orders", "Trade");
    }

    [Route("/sellorder")]
    [TypeFilter<CreateOrderActionFilter>()]
    public async Task<ActionResult> SellOrder([FromForm] BaseOrder incomingOrder)
    {
        logger.Information("Entered TradeController's SellOrder.");

        await stocksService.CreateSellOrder(incomingOrder.ToSellRequest());

        return RedirectToAction("Orders", "Trade");
    }
}
