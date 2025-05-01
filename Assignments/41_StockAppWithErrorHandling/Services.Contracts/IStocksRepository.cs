using Models.DTO.Order;
using Models.Entities.Order;

namespace Services.Contracts;

public interface IStocksRepository
{
    public Task<BuyOrder> AddBuyOrder(BuyOrder buyOrder);

    public Task<SellOrder> AddSellOrder(SellOrder sellOrder);

    public List<BuyResponse> GetBuyOrders();

    public List<SellResponse> GetSellOrders();
}
