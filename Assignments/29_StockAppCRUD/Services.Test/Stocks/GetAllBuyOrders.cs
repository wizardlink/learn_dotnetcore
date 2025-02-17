using Models.DTO.Order;

namespace Services.Test.Stocks;

public class GetAllBuyOrders
{
    private readonly StocksService _stocksService;

    public GetAllBuyOrders()
    {
        _stocksService = new StocksService();
    }

    [Fact]
    public void EmptyByDefault()
    {
        var buyOrders = _stocksService.GetBuyOrders();

        Assert.Empty(buyOrders);
    }

    [Fact]
    public void ValidValues()
    {
        var data = new BuyRequest
        {
            StockSymbol = "MSFT",
            Price = 100,
            Quantity = 100,
            DateAndTimeOfOrder = DateTime.Parse("2001-01-01"),
        };
        var firstBuyOrder = _stocksService.CreateBuyOrder(data);

        data.StockSymbol = "GOOG";
        var secondBuyOrder = _stocksService.CreateBuyOrder(data);

        var buyOrders = _stocksService.GetBuyOrders();

        Assert.Contains(firstBuyOrder, buyOrders);
        Assert.Contains(secondBuyOrder, buyOrders);
    }
}
