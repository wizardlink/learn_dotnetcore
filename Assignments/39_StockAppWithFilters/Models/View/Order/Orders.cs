using Models.DTO.Order;

namespace Models.View.Order;

public class Orders
{
    public required List<BuyResponse> BuyResponses { get; set; }
    public required List<SellResponse> SellResponses { get; set; }
}
