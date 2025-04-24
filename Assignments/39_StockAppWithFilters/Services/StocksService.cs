using Models.DTO.Order;
using Serilog;
using SerilogTimings;
using Services.Contracts;

namespace Services;

public class StocksService(IStocksRepository stocksRepository, IDiagnosticContext diagnosticContext) : IStocksService
{
    public async Task<BuyResponse> CreateBuyOrder(BuyRequest? buyRequest)
    {
        ArgumentNullException.ThrowIfNull(buyRequest);

        var buyOrder = buyRequest.ToBuyOrder();

        await stocksRepository.AddBuyOrder(buyOrder);

        return buyOrder.ToBuyResponse();
    }

    public async Task<SellResponse> CreateSellOrder(SellRequest? sellRequest)
    {
        ArgumentNullException.ThrowIfNull(sellRequest);

        var sellOrder = sellRequest.ToSellOrder();

        await stocksRepository.AddSellOrder(sellOrder);

        return sellOrder.ToSellResponse();
    }

    public List<BuyResponse> GetBuyOrders()
    {
        List<BuyResponse> buyOrders = [];

        using (Operation.Time("Time for GetBuyOrders"))
            buyOrders = stocksRepository.GetBuyOrders();

        diagnosticContext.Set("BuyOrders", buyOrders, true);

        return buyOrders;
    }

    public List<SellResponse> GetSellOrders()
    {
        List<SellResponse> sellOrders = [];

        using (Operation.Time("Time for GetSellOrders"))
            sellOrders = stocksRepository.GetSellOrders();

        diagnosticContext.Set("SellOrders", sellOrders, true);

        return sellOrders;
    }
}
