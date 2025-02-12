using Models.DTO.Order;

namespace Services.Test.Stocks;

public class CreateSellOrder
{
    private readonly StocksService _stocksService;

    public CreateSellOrder()
    {
        _stocksService = new StocksService();
    }

    [Fact]
    public async void NullBuyOrder()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _stocksService.CreateSellOrder(null));
    }

    [Fact]
    public async void ZeroQuantity()
    {
        var data = new SellRequest
        {
            Quantity = 0,
            StockSymbol = "MSFT",
            Price = 500,
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(data));
    }

    [Fact]
    public async void OverMaxQuantity()
    {
        var data = new SellRequest
        {
            Quantity = 100001,
            StockSymbol = "MSFT",
            Price = 500,
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(data));
    }

    [Fact]
    public async void ZeroPrice()
    {
        var data = new SellRequest
        {
            Price = 0,
            StockSymbol = "MSFT",
            Quantity = 100,
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(data));
    }

    [Fact]
    public async void OverMaxPrice()
    {
        var data = new SellRequest
        {
            Price = 10001,
            StockSymbol = "MSFT",
            Quantity = 100,
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(data));
    }

    [Fact]
    public async void NullStockSymbol()
    {
        var data = new SellRequest
        {
            StockSymbol = null,
            Price = 100,
            Quantity = 100,
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(data));
    }

    [Fact]
    public async void BadDate()
    {
        var data = new SellRequest
        {
            StockSymbol = null,
            Price = 100,
            Quantity = 100,
            DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(data));
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

        var buyOrder = await _stocksService.CreateSellOrder(data);

        Assert.IsType<SellResponse>(buyOrder);
    }
}
