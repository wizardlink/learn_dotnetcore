using AutoFixture;
using Moq;
using Services.Contracts;

namespace Services.Test.Controllers;

public abstract class BaseClass
{
    protected readonly Fixture _fixture = new();
    protected readonly IFinnhubService _finnhubService;
    protected readonly IStocksService _stocksService;
    protected readonly Mock<IFinnhubService> _mockFinnhub = new();
    protected readonly Mock<IStocksService> _mockStocks = new();

    public BaseClass()
    {
        _stocksService = _mockStocks.Object;
        _finnhubService = _mockFinnhub.Object;
    }
}
