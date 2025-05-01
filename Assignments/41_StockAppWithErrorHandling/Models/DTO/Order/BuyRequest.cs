using Models.Entities.Order;

namespace Models.DTO.Order;

public class BuyRequest : BaseOrder
{
    public new string? StockSymbol { get; set; }
}

public static partial class BaseOrderExtensions
{
    public static BuyRequest ToBuyRequest(this BaseOrder order)
    {
        return new BuyRequest
        {
            Price = order.Price,
            Quantity = order.Quantity,
            StockName = order.StockName,
            StockSymbol = order.StockSymbol,
        };
    }
}
