using AutoFixture;
using FluentAssertions;
using Models.DTO.Order;
using Models.Entities.Order;
using Moq;

namespace Services.Test.Stocks;

public class GetAllBuyOrders : BaseClass
{
    [Fact]
    public void EmptyByDefault()
    {
        var buyOrders = _service.GetBuyOrders();

        buyOrders.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task ValidValues()
    {
        List<BuyRequest> requests = [_fixture.Create<BuyRequest>(), _fixture.Create<BuyRequest>()];
        List<BuyResponse> responses = [];

        foreach (var request in requests)
        {
            _mock.Setup(repo => repo.AddBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(request.ToBuyOrder());
            responses.Add(await _service.CreateBuyOrder(request));
        }

        _mock.Setup(repo => repo.GetBuyOrders()).Returns(responses);
        var buyOrders = _service.GetBuyOrders();

        buyOrders.Should().IntersectWith(responses);
    }
}
