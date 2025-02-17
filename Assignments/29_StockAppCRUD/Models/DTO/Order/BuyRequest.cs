using Models.Entities.Order;

namespace Models.DTO.Order;

public class BuyRequest : BaseOrder
{
    public new string? StockSymbol { get; set; }
}
