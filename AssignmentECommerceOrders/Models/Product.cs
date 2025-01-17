using System.ComponentModel.DataAnnotations;

namespace AssignmentECommerceOrders.Models;

public class Product
{
    [Required(ErrorMessage = "{0} cannot be empty!")]
    public int ProductCode { get; set; }

    [Required(ErrorMessage = "{0} cannot be empty!")]
    public double Price { get; set; }

    [Required(ErrorMessage = "{0} cannot be empty!")]
    public int Quantity { get; set; }
}
