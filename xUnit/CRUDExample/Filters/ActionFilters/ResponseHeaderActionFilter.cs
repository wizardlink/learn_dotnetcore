using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ResponseHeaderFilterFactory : Attribute, IFilterFactory
{
    public bool IsReusable => false;

    private string Key { get; }
    private string Value { get; }
    private int Order { get; } = 0;

    public ResponseHeaderFilterFactory(string key, string value, int order = 0)
    {
        Key = key;
        Value = value;
        Order = order;
    }

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        return new ResponseHeaderActionFilter(Key, Value, Order);
    }
}

public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
{
    private readonly string _key;
    private readonly string _value;

    public int Order { get; }

    public ResponseHeaderActionFilter(string key, string value, int order = 0) =>
        (_key, _value, Order) = (key, value, order);

    ///<summary>
    /// We create a class instead of an override because dependency injection does not allow overloads,
    /// plus it would not figure out what are the `string` parameters.
    ///</summary>
    public class Global() : ResponseHeaderActionFilter("My-Key-From-Global", "My-Value-From-Global") { }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.HttpContext.Response.Headers[_key] = _value;

        await next();
    }
}
