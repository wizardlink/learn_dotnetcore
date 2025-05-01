using System.ComponentModel.DataAnnotations;

namespace Models.Entities.Order;

public class BuyOrder : BaseOrder
{
    [Key]
    public Guid BuyOrderId { get; set; }
}
