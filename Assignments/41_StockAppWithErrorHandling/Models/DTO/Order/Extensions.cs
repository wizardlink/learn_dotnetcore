using Models.Entities.Order;

namespace Models.DTO.Order;

public static class Extensions
{
    #region Buy
    public static BuyOrder ToBuyOrder(this BuyRequest request)
    {
        ArgumentException.ThrowIfNullOrEmpty(request.StockSymbol);

        return new BuyOrder
        {
            BuyOrderId = Guid.NewGuid(),
            DateAndTimeOfOrder = DateTime.Now,
            Price = request.Price,
            Quantity = request.Quantity,
            StockName = request.StockName,
            StockSymbol = request.StockSymbol,
        };
    }

    public static BuyResponse ToBuyResponse(this BuyOrder order)
    {
        return new BuyResponse
        {
            DateAndTimeOfOrder = order.DateAndTimeOfOrder,
            Price = order.Price,
            Quantity = order.Quantity,
            StockName = order.StockName,
            StockSymbol = order.StockSymbol,
            TradeAmount = order.Quantity * order.Price,
        };
    }
    #endregion

    #region Sell
    public static SellOrder ToSellOrder(this SellRequest request)
    {
        ArgumentException.ThrowIfNullOrEmpty(request.StockSymbol);

        return new SellOrder
        {
            SellOrderID = Guid.NewGuid(),
            DateAndTimeOfOrder = DateTime.Now,
            Price = request.Price,
            Quantity = request.Quantity,
            StockName = request.StockName,
            StockSymbol = request.StockSymbol,
        };
    }

    public static SellResponse ToSellResponse(this SellOrder order)
    {
        return new SellResponse
        {
            DateAndTimeOfOrder = order.DateAndTimeOfOrder,
            Price = order.Price,
            Quantity = order.Quantity,
            StockName = order.StockName,
            StockSymbol = order.StockSymbol,
            TradeAmount = order.Quantity * order.Price,
        };
    }
    #endregion
}
