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
    public async void EmptyByDefault()
    {
        var buyOrders = await _stocksService.GetSellOrders();

        Assert.Empty(buyOrders);
    }

    [Fact]
    public async void ValidValues()
    {
        var data = new SellRequest
        {
            StockSymbol = "MSFT",
            Price = 100,
            Quantity = 100,
            DateAndTimeOfOrder = DateTime.Parse("2001-01-01"),
        };
        var firstSellOrder = await _stocksService.CreateSellOrder(data);

        data.StockSymbol = "GOOG";
        var secondSellOrder = await _stocksService.CreateSellOrder(data);

        var buyOrders = await _stocksService.GetSellOrders();

        Assert.Contains(firstSellOrder, buyOrders);
        Assert.Contains(secondSellOrder, buyOrders);
    }
}
