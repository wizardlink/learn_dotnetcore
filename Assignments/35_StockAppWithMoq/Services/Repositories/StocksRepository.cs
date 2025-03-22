using Models.DTO.Order;
using Models.Entities.Order;
using Services.Contracts;

namespace Services.Repositories;

public class StocksRepository(DatabaseContext databaseContext) : IStocksRepository
{
    public async Task<BuyOrder> AddBuyOrder(BuyOrder buyOrder)
    {
        databaseContext.BuyOrder.Add(buyOrder);
        await databaseContext.SaveChangesAsync();

        return buyOrder;
    }

    public async Task<SellOrder> AddSellOrder(SellOrder sellOrder)
    {
        databaseContext.SellOrder.Add(sellOrder);
        await databaseContext.SaveChangesAsync();

        return sellOrder;
    }

    public List<BuyResponse> GetBuyOrders()
    {
        return [.. databaseContext.BuyOrder.Select(order => order.ToBuyResponse())];
    }

    public List<SellResponse> GetSellOrders()
    {
        return [.. databaseContext.SellOrder.Select(order => order.ToSellResponse())];
    }
}
