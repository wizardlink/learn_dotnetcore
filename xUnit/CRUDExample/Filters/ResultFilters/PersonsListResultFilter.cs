using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResultFilters;

public class PersonsListResultFilter(ILogger<PersonsListResultFilter> logger) : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        logger.LogInformation(
            "{FilterName}.{MethodName} - before",
            nameof(PersonsListResultFilter),
            nameof(OnResultExecutionAsync)
        );

        context.HttpContext.Response.Headers.LastModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        await next();

        logger.LogInformation(
            "{FilterName}.{MethodName} - after",
            nameof(PersonsListResultFilter),
            nameof(OnResultExecutionAsync)
        );
    }
}
