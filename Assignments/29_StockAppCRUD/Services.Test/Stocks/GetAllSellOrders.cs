using Models.DTO.Order;

namespace Services.Test.Stocks;

public class GetAllSellOrders
{
    private readonly StocksService _stocksService;

    public GetAllSellOrders()
    {
        _stocksService = new StocksService();
    }

    [Fact]
    public void EmptyByDefault()
    {
        var buyOrders = _stocksService.GetSellOrders();

        Assert.Empty(buyOrders);
    }

    [Fact]
    public void ValidValues()
    {
        var data = new SellRequest
        {
            StockSymbol = "MSFT",
            Price = 100,
            Quantity = 100,
            DateAndTimeOfOrder = DateTime.Parse("2001-01-01"),
        };
        var firstSellOrder = _stocksService.CreateSellOrder(data);

        data.StockSymbol = "GOOG";
        var secondSellOrder = _stocksService.CreateSellOrder(data);

        var buyOrders = _stocksService.GetSellOrders();

        Assert.Contains(firstSellOrder, buyOrders);
        Assert.Contains(secondSellOrder, buyOrders);
    }
}
