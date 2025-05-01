using System.ComponentModel.DataAnnotations;

namespace Models.Entities.Order;

public class BaseOrder
{
    [Required]
    public string StockSymbol { get; set; } = string.Empty;

    public string StockName { get; set; } = string.Empty;

    [AtLeastDate("2000-01-01")]
    public DateTime DateAndTimeOfOrder { get; set; } = DateTime.Now;

    [Range(1, 100000)]
    public uint Quantity { get; set; }

    [Range(1, 10000)]
    public double Price { get; set; }
}
