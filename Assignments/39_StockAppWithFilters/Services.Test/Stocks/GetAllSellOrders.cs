using AutoFixture;
using FluentAssertions;
using Models.DTO.Order;
using Models.Entities.Order;
using Moq;

namespace Services.Test.Stocks;

public class GetAllSellOrders : BaseClass
{
    [Fact]
    public void EmptyByDefault()
    {
        var sellOrders = _service.GetSellOrders();

        sellOrders.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task ValidValues()
    {
        List<SellRequest> requests = [_fixture.Create<SellRequest>(), _fixture.Create<SellRequest>()];
        List<SellResponse> responses = [];

        foreach (var request in requests)
        {
            _mock.Setup(repo => repo.AddSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(request.ToSellOrder());
            responses.Add(await _service.CreateSellOrder(request));
        }

        _mock.Setup(repo => repo.GetSellOrders()).Returns(responses);
        var sellOrders = _service.GetSellOrders();

        sellOrders.Should().IntersectWith(responses);
    }
}
