using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Services.Contracts;

namespace Services.Test.Stocks;

public abstract class BaseClass
{
    protected readonly Fixture _fixture = new();
    protected readonly IStocksRepository _repository;
    protected readonly IStocksService _service;
    protected readonly Mock<IStocksRepository> _mock = new();

    public BaseClass()
    {
        _repository = _mock.Object;

        var dbContextMock = new Mock<DatabaseContext>(new DbContextOptionsBuilder<DatabaseContext>().Options);

        DatabaseContext dbContext = dbContextMock.Object;

        dbContextMock.Setup(db => db.BuyOrder).ReturnsDbSet([]);
        dbContextMock.Setup(db => db.SellOrder).ReturnsDbSet([]);

        _service = new StocksService(_repository);
    }
}
