using Models.DTO.Order;

namespace Services.Test.Stocks;

public class CreateBuyOrder
{
    private readonly StocksService _stocksService;

    public CreateBuyOrder()
    {
        _stocksService = new StocksService();
    }

    [Fact]
    public void NullBuyOrder()
    {
        Assert.Throws<ArgumentNullException>(() => _stocksService.CreateBuyOrder(null));
    }

    [Fact]
    public void ZeroQuantity()
    {
        var data = new BuyRequest
        {
            Quantity = 0,
            StockSymbol = "MSFT",
            Price = 500,
        };

        Assert.Throws<ArgumentException>(() => _stocksService.CreateBuyOrder(data));
    }

    [Fact]
    public void OverMaxQuantity()
    {
        var data = new BuyRequest
        {
            Quantity = 100001,
            StockSymbol = "MSFT",
            Price = 500,
        };

        Assert.Throws<ArgumentException>(() => _stocksService.CreateBuyOrder(data));
    }

    [Fact]
    public void ZeroPrice()
    {
        var data = new BuyRequest
        {
            Price = 0,
            StockSymbol = "MSFT",
            Quantity = 100,
        };

        Assert.Throws<ArgumentException>(() => _stocksService.CreateBuyOrder(data));
    }

    [Fact]
    public void OverMaxPrice()
    {
        var data = new BuyRequest
        {
            Price = 10001,
            StockSymbol = "MSFT",
            Quantity = 100,
        };

        Assert.Throws<ArgumentException>(() => _stocksService.CreateBuyOrder(data));
    }

    [Fact]
    public void NullStockSymbol()
    {
        var data = new BuyRequest
        {
            StockSymbol = null,
            Price = 100,
            Quantity = 100,
        };

        Assert.Throws<ArgumentException>(() => _stocksService.CreateBuyOrder(data));
    }

    [Fact]
    public void BadDate()
    {
        var data = new BuyRequest
        {
            StockSymbol = null,
            Price = 100,
            Quantity = 100,
            DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
        };

        Assert.Throws<ArgumentException>(() => _stocksService.CreateBuyOrder(data));
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

        var buyOrder = _stocksService.CreateBuyOrder(data);

        Assert.IsType<BuyResponse>(buyOrder);
    }
}
