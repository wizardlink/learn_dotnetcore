using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Models.DTO.Finnhub;
using Models.View.Order;
using Moq;
using WebApp.Configuration;
using WebApp.Controllers;

namespace Services.Test.Controllers;

public class Trade : BaseClass
{
    private readonly IConfiguration _configuration;
    private readonly IOptions<TradingOptions> _options;
    private readonly TradeController _controller;

    public Trade()
        : base()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        _options = Options.Create(new TradingOptions { DefaultStockSymbol = "MSFT" });

        _controller = new(_options, _configuration, _finnhubService, _stocksService);
    }

    [Fact]
    public async Task FailedGetFinnhub()
    {
        var actionResult = await _controller.Index(string.Empty);

        actionResult.Should().BeOfType<StatusCodeResult>();
    }

    [Fact]
    public async Task ListOfStocks()
    {
        _mockFinnhub
            .Setup(service => service.GetCompanyProfile(It.IsAny<string>()))
            .ReturnsAsync(_fixture.Create<CompanyProfile>());

        _mockFinnhub
            .Setup(service => service.GetStockPriceQuote(It.IsAny<string>()))
            .ReturnsAsync(_fixture.Create<StockPriceQuote>());

        var actionResult = await _controller.Index(string.Empty);

        actionResult.Should().BeOfType<ViewResult>();

        var viewData = ((ViewResult)actionResult).ViewData;

        viewData.Model.Should().BeAssignableTo<StockTrade>();
    }

    [Fact]
    public void ListOfOrders()
    {
        var actionResult = _controller.Orders();

        actionResult.Should().BeOfType<ViewResult>();

        var viewData = ((ViewResult)actionResult).ViewData;

        viewData.Model.Should().BeAssignableTo<Orders>();
    }

    [Fact]
    public async Task AddBuyOrder()
    {
        var actionResult = await _controller.BuyOrder("100", "100", "GOOG", "Google");

        actionResult.Should().BeAssignableTo<RedirectToActionResult>();
    }

    [Fact]
    public async Task AddSellOrder()
    {
        var actionResult = await _controller.SellOrder("100", "100", "GOOG", "Google");

        actionResult.Should().BeAssignableTo<RedirectToActionResult>();
    }
}
