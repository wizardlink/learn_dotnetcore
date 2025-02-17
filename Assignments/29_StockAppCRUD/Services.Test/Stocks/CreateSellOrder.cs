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
    public void NullBuyOrder()
    {
        Assert.Throws<ArgumentNullException>(() => _stocksService.CreateSellOrder(null));
    }

    [Fact]
    public void ZeroQuantity()
    {
        var data = new SellRequest
        {
            Quantity = 0,
            StockSymbol = "MSFT",
            Price = 500,
        };

        Assert.Throws<ArgumentException>(() => _stocksService.CreateSellOrder(data));
    }

    [Fact]
    public void OverMaxQuantity()
    {
        var data = new SellRequest
        {
            Quantity = 100001,
            StockSymbol = "MSFT",
            Price = 500,
        };

        Assert.Throws<ArgumentException>(() => _stocksService.CreateSellOrder(data));
    }

    [Fact]
    public void ZeroPrice()
    {
        var data = new SellRequest
        {
            Price = 0,
            StockSymbol = "MSFT",
            Quantity = 100,
        };

        Assert.Throws<ArgumentException>(() => _stocksService.CreateSellOrder(data));
    }

    [Fact]
    public void OverMaxPrice()
    {
        var data = new SellRequest
        {
            Price = 10001,
            StockSymbol = "MSFT",
            Quantity = 100,
        };

        Assert.Throws<ArgumentException>(() => _stocksService.CreateSellOrder(data));
    }

    [Fact]
    public void NullStockSymbol()
    {
        var data = new SellRequest
        {
            StockSymbol = null,
            Price = 100,
            Quantity = 100,
        };

        Assert.Throws<ArgumentException>(() => _stocksService.CreateSellOrder(data));
    }

    [Fact]
    public void BadDate()
    {
        var data = new SellRequest
        {
            StockSymbol = null,
            Price = 100,
            Quantity = 100,
            DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
        };

        Assert.Throws<ArgumentException>(() => _stocksService.CreateSellOrder(data));
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

        var buyOrder = _stocksService.CreateSellOrder(data);

        Assert.IsType<SellResponse>(buyOrder);
    }
}
