using Models;
using Models.DTO.Order;
using Services.Contracts;

namespace Services;

public class StocksService : IStocksService
{
    private readonly List<BuyResponse> _buyResponseList = [];
    private readonly List<SellResponse> _sellResponseList = [];

    public BuyResponse CreateBuyOrder(BuyRequest? buyRequest)
    {
        ArgumentNullException.ThrowIfNull(buyRequest);
        Validations.ComponentModelValidation(buyRequest);

        var response = new BuyResponse
        {
            StockSymbol = buyRequest.StockSymbol!,
            StockName = buyRequest.StockName,
            DateAndTimeOfOrder = buyRequest.DateAndTimeOfOrder,
            Quantity = buyRequest.Quantity,
            Price = buyRequest.Price,
        };

        _buyResponseList.Add(response);

        return response;
    }

    public SellResponse CreateSellOrder(SellRequest? sellRequest)
    {
        ArgumentNullException.ThrowIfNull(sellRequest);
        Validations.ComponentModelValidation(sellRequest);

        var response = new SellResponse
        {
            StockSymbol = sellRequest.StockSymbol!,
            StockName = sellRequest.StockName,
            DateAndTimeOfOrder = sellRequest.DateAndTimeOfOrder,
            Quantity = sellRequest.Quantity,
            Price = sellRequest.Price,
        };

        _sellResponseList.Add(response);

        return response;
    }

    public List<BuyResponse> GetBuyOrders()
    {
        return _buyResponseList;
    }

    public List<SellResponse> GetSellOrders()
    {
        return _sellResponseList;
    }
}
