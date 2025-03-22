using Models.DTO.Order;

namespace Services.Contracts;

public interface IStocksService
{
    public Task<BuyResponse> CreateBuyOrder(BuyRequest? buyRequest);

    public Task<SellResponse> CreateSellOrder(SellRequest? sellRequest);

    public List<BuyResponse> GetBuyOrders();

    public List<SellResponse> GetSellOrders();
}
