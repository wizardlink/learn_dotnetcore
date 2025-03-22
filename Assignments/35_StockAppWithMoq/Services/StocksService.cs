using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Order;
using Services.Contracts;

namespace Services;

public class StocksService(IStocksRepository stocksRepository) : IStocksService
{
    public async Task<BuyResponse> CreateBuyOrder(BuyRequest? buyRequest)
    {
        ArgumentNullException.ThrowIfNull(buyRequest);
        Validations.ComponentModelValidation(buyRequest);

        var buyOrder = buyRequest.ToBuyOrder();

        await stocksRepository.AddBuyOrder(buyOrder);

        return buyOrder.ToBuyResponse();
    }

    public async Task<SellResponse> CreateSellOrder(SellRequest? sellRequest)
    {
        ArgumentNullException.ThrowIfNull(sellRequest);
        Validations.ComponentModelValidation(sellRequest);

        var sellOrder = sellRequest.ToSellOrder();

        await stocksRepository.AddSellOrder(sellOrder);

        return sellOrder.ToSellResponse();
    }

    public List<BuyResponse> GetBuyOrders()
    {
        return stocksRepository.GetBuyOrders();
    }

    public List<SellResponse> GetSellOrders()
    {
        return stocksRepository.GetSellOrders();
    }
}
