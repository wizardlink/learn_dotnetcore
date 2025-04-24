using AutoFixture;
using FluentAssertions;
using Models.DTO.Order;
using Models.Entities.Order;
using Moq;

namespace Services.Test.Stocks;

public class CreateSellOrder : BaseClass
{
    [Fact]
    public async Task NullBuyOrder()
    {
        Func<Task> action = () =>
        {
            return _service.CreateSellOrder(null);
        };

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task ZeroQuantity()
    {
        var data = _fixture.Build<SellRequest>().With(req => req.Quantity, (uint)0).Create();

        Func<Task> action = () =>
        {
            return _service.CreateSellOrder(data);
        };

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task OverMaxQuantity()
    {
        var data = _fixture.Build<SellRequest>().With(req => req.Quantity, (uint)100001).Create();

        Func<Task> action = () =>
        {
            return _service.CreateSellOrder(data);
        };

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task ZeroPrice()
    {
        var data = _fixture.Build<SellRequest>().With(req => req.Price, 0).Create();

        Func<Task> action = () =>
        {
            return _service.CreateSellOrder(data);
        };

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task OverMaxPrice()
    {
        var data = _fixture.Build<SellRequest>().With(req => req.Price, 10001).Create();

        Func<Task> action = () =>
        {
            return _service.CreateSellOrder(data);
        };

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task NullStockSymbol()
    {
        var data = _fixture.Build<SellRequest>().Without(req => req.StockSymbol).Create();

        Func<Task> action = () =>
        {
            return _service.CreateSellOrder(data);
        };

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task BadDate()
    {
        var data = _fixture
            .Build<SellRequest>()
            .With(req => req.DateAndTimeOfOrder, DateTime.Parse("1999-12-31"))
            .Create();

        Func<Task> action = () =>
        {
            return _service.CreateSellOrder(data);
        };

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task ValidValues()
    {
        var data = _fixture.Create<SellRequest>();

        var sellOrder = data.ToSellOrder();

        _mock.Setup(repo => repo.AddSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

        var request = await _service.CreateSellOrder(data);

        request.Should().BeAssignableTo<SellResponse>();
    }
}
