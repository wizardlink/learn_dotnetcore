using Models.Entities.Order;

namespace Models.DTO.Order;

public class SellRequest : BaseOrder
{
    public new string? StockSymbol { get; set; }
}

public static partial class BaseOrderExtensions
{
    public static SellRequest ToSellRequest(this BaseOrder order)
    {
        return new SellRequest
        {
            Price = order.Price,
            Quantity = order.Quantity,
            StockName = order.StockName,
            StockSymbol = order.StockSymbol,
        };
    }
}
