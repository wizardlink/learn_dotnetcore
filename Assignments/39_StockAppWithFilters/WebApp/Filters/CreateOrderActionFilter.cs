using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Entities.Order;
using WebApp.Controllers;

namespace WebApp.Filters;

public class CreateOrderActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.Controller is not TradeController)
        {
            await next();
            return;
        }

        TradeController controller = (TradeController)context.Controller;

        bool hasOrder = context.ActionArguments.TryGetValue("incomingOrder", out object? incomingOrder);

        if (!hasOrder || incomingOrder == null)
        {
            await next();
            return;
        }

        BaseOrder order = (BaseOrder)incomingOrder;

        ValidationContext validationContext = new(order);
        List<ValidationResult> validationResults = [];

        bool isValid = Validator.TryValidateObject(order, validationContext, validationResults, true);

        if (isValid)
        {
            await next();
            return;
        }

        controller.ViewData["Errors"] = validationResults.Select(r => r.ErrorMessage);

        context.Result = controller.View(nameof(TradeController.Index), order);
    }
}
