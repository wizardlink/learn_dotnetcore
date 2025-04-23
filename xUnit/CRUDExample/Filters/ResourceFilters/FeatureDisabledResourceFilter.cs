using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResourceFilters;

public class FeatureDisabledResourceFilter(ILogger<FeatureDisabledResourceFilter> logger, bool isDisabled = true)
    : IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        logger.LogInformation(
            "{FilterName}.{MethodName} - before",
            nameof(FeatureDisabledResourceFilter),
            nameof(OnResourceExecutionAsync)
        );

        if (isDisabled)
            context.Result = new NotFoundResult();

        if (context.Result is EmptyResult)
            await next();

        logger.LogInformation(
            "{FilterName}.{MethodName} - after",
            nameof(FeatureDisabledResourceFilter),
            nameof(OnResourceExecutionAsync)
        );
    }
}
