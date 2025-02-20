using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Order;
using Services.Contracts;

namespace Services;

public class StocksService(DatabaseContext databaseContext) : IStocksService
{
    private readonly DatabaseContext _database = databaseContext;

    public async Task<BuyResponse> CreateBuyOrder(BuyRequest? buyRequest)
    {
        ArgumentNullException.ThrowIfNull(buyRequest);
        Validations.ComponentModelValidation(buyRequest);

        var buyOrder = buyRequest.ToBuyOrder();

        _database.BuyOrder.Add(buyOrder);
        await _database.SaveChangesAsync();

        return buyOrder.ToBuyResponse();
    }

    public async Task<SellResponse> CreateSellOrder(SellRequest? sellRequest)
    {
        ArgumentNullException.ThrowIfNull(sellRequest);
        Validations.ComponentModelValidation(sellRequest);

        var sellOrder = sellRequest.ToSellOrder();

        _database.SellOrder.Add(sellOrder);
        await _database.SaveChangesAsync();

        return sellOrder.ToSellResponse();
    }

    public Task<List<BuyResponse>> GetBuyOrders()
    {
        return _database.BuyOrder.Select(order => order.ToBuyResponse()).ToListAsync();
    }

    public Task<List<SellResponse>> GetSellOrders()
    {
        return _database.SellOrder.Select(order => order.ToSellResponse()).ToListAsync();
    }
}
