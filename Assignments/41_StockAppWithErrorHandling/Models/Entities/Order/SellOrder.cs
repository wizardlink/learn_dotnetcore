using System.ComponentModel.DataAnnotations;

namespace Models.Entities.Order;

public class SellOrder : BaseOrder
{
    [Key]
    public Guid SellOrderID { get; set; }
}
