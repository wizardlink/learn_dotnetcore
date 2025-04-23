using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters;

public class ResponseHeaderActionFilter : ActionFilterAttribute
{
    private readonly string _key;
    private readonly string _value;

    public ResponseHeaderActionFilter(string key, string value, int order = 0) =>
        (_key, _value, Order) = (key, value, order);

    ///<summary>
    /// We create a class instead of an override because dependency injection does not allow overloads,
    /// plus it would not figure out what are the `string` parameters.
    ///</summary>
    public class Global() : ResponseHeaderActionFilter("My-Key-From-Global", "My-Value-From-Global") { }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.HttpContext.Response.Headers[_key] = _value;

        await next();
    }
}
