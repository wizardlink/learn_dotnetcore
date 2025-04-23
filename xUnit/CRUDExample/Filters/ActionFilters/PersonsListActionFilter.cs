using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace CRUDExample.Filters.ActionFilters;

public class PersonsListActionFilter(ILogger<PersonsListActionFilter> logger) : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        logger.LogInformation(
            "{FilterName}.{MethodName} method",
            nameof(PersonsListActionFilter),
            nameof(OnActionExecuted)
        );

        bool hasActionArguments = context.HttpContext.Items.TryGetValue(
            "ActionArguments",
            out object? contextActionArguments
        );
        if (!hasActionArguments || contextActionArguments == null)
            return;

        var actionArguments = (IDictionary<string, object?>)contextActionArguments;

        var controller = (PersonsController)context.Controller;

        if (actionArguments.TryGetValue("searchBy", out object? searchBy))
            controller.ViewData["CurrentSearchBy"] = searchBy;

        if (actionArguments.TryGetValue("searchString", out object? searchString))
            controller.ViewData["CurrentSearchString"] = searchString;

        if (actionArguments.TryGetValue("sortBy", out object? sortBy))
            controller.ViewData["CurrentSortBy"] = sortBy;

        if (actionArguments.TryGetValue("sortOrder", out object? sortOrder))
            controller.ViewData["CurrentSortOrder"] = sortOrder;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        logger.LogInformation(
            "{FilterName}.{MethodName} method",
            nameof(PersonsListActionFilter),
            nameof(OnActionExecuting)
        );

        context.HttpContext.Items["ActionArguments"] = context.ActionArguments;

        if (context.ActionArguments.TryGetValue("searchBy", out object? value))
        {
            string? searchBy = value?.ToString();

            if (!string.IsNullOrEmpty(searchBy))
            {
                string[] options =
                [
                    nameof(PersonResponse.PersonName),
                    nameof(PersonResponse.Email),
                    nameof(PersonResponse.DateOfBirth),
                    nameof(PersonResponse.Gender),
                    nameof(PersonResponse.CountryID),
                    nameof(PersonResponse.Address),
                ];

                if (!options.Any(option => option == searchBy))
                {
                    logger.LogInformation("searchBy actual value {searchBy}", searchBy);
                    context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                }
            }
        }
    }
}
