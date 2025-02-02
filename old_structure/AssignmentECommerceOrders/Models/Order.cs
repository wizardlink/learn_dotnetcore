using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AssignmentECommerceOrders.Models;

public class Order : IValidatableObject
{
    [BindNever]
    public int? OrderNo { get; set; }

    [Required(ErrorMessage = "{0} cannot be empty!")]
    public DateTime OrderDate { get; set; }

    [Required(ErrorMessage = "{0} cannot be empty!")]
    public double InvoicePrice { get; set; }

    [MinLength(1, ErrorMessage = "{0} needs at least one item.")]
    public List<Product> Products { get; set; } = new List<Product>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        double totalPrice = 0;
        Products.ForEach(order =>
        {
            totalPrice += order.Price * order.Quantity;
        });

        if (totalPrice != InvoicePrice)
        {
            yield return new ValidationResult(
                "InvoicePrice doesn't match with the total cost of the specified products in the order."
            );
        }

        if (OrderDate < new DateTime(2000, 01, 01))
        {
            yield return new ValidationResult(
                "OrderDate must be greater than 2000-01-01."
            );
        }
    }
}
