using Models.Entities.Order;

namespace Models.DTO.Order;

public class SellRequest : BaseOrder
{
    public new string? StockSymbol { get; set; }
}
