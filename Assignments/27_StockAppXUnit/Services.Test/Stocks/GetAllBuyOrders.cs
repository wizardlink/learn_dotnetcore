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
    public async void EmptyByDefault()
    {
        var buyOrders = await _stocksService.GetBuyOrders();

        Assert.Empty(buyOrders);
    }

    [Fact]
    public async void ValidValues()
    {
        var data = new BuyRequest
        {
            StockSymbol = "MSFT",
            Price = 100,
            Quantity = 100,
            DateAndTimeOfOrder = DateTime.Parse("2001-01-01"),
        };
        var firstBuyOrder = await _stocksService.CreateBuyOrder(data);

        data.StockSymbol = "GOOG";
        var secondBuyOrder = await _stocksService.CreateBuyOrder(data);

        var buyOrders = await _stocksService.GetBuyOrders();

        Assert.Contains(firstBuyOrder, buyOrders);
        Assert.Contains(secondBuyOrder, buyOrders);
    }
}
