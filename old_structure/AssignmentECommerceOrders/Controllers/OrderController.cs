using AssignmentECommerceOrders.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentECommerceOrders.Controllers;

public class OrderController : Controller
{
    [Route("order")]
    public IActionResult Index([FromForm] Order order)
    {
        if (!ModelState.IsValid)
        {
            string error = string.Join(
                "\n",
                ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage)
            );

            return BadRequest(error);
        }

        return Json(new { OrderNo = new Random().Next(1, 10000) });
    }
}
