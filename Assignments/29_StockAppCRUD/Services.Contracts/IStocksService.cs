using Models.DTO.Order;

namespace Services.Contracts;

public interface IStocksService
{
    public BuyResponse CreateBuyOrder(BuyRequest? buyRequest);

    public SellResponse CreateSellOrder(SellRequest? sellRequest);

    public List<BuyResponse> GetBuyOrders();

    public List<SellResponse> GetSellOrders();
}
