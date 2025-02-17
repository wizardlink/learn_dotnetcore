using Models.Entities.Order;

namespace Models.DTO.Order;

public class BaseResponse : BaseOrder
{
    public double TradeAmount { get; set; }
}
