using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Models.DTO.Finnhub;
using Moq;
using WebApp.Controllers;

namespace Services.Test.Controllers;

public class Explore : BaseClass
{
    private readonly ExploreController _controller;

    public Explore()
    {
        _controller = new(_finnhubService);
    }

    [Fact]
    public async Task ListOfStocks()
    {
        _mockFinnhub.Setup(service => service.GetStocks()).ReturnsAsync([.. _fixture.CreateMany<Stock>()]);

        var actionResult = await _controller.Index(string.Empty, false);

        actionResult.Should().BeOfType<ViewResult>();

        var viewData = ((ViewResult)actionResult).ViewData;

        viewData.Model.Should().BeAssignableTo<IEnumerable<Stock>>();
    }

    [Fact]
    public async Task ListWithStockInfo()
    {
        _mockFinnhub.Setup(service => service.GetStocks()).ReturnsAsync([.. _fixture.CreateMany<Stock>()]);
        _mockFinnhub
            .Setup(service => service.GetCompanyProfile(It.IsAny<string>()))
            .ReturnsAsync(_fixture.Create<CompanyProfile>());
        _mockFinnhub
            .Setup(service => service.GetStockPriceQuote(It.IsAny<string>()))
            .ReturnsAsync(_fixture.Create<StockPriceQuote>());

        var actionResult = await _controller.Index("GOOG", false);

        actionResult.Should().BeOfType<ViewResult>();

        var viewData = ((ViewResult)actionResult).ViewData;

        viewData.Model.Should().BeAssignableTo<IEnumerable<Stock>>();

        var selectedCompany = viewData.SingleOrDefault(data => data.Key == "SelectedCompany").Value;
        var selectedPrice = viewData.SingleOrDefault(data => data.Key == "SelectedPrice").Value;

        selectedCompany.Should().BeAssignableTo<CompanyProfile>();
        selectedPrice.Should().BeAssignableTo<StockPriceQuote>();
    }
}
