using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;

namespace CRUDExample.Filters.ActionFilters;

public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
{
    private readonly ICountriesService _countriesService;

    public PersonCreateAndEditPostActionFilter(ICountriesService countriesService) =>
        _countriesService = countriesService;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.Controller is PersonsController controller && !controller.ModelState.IsValid)
        {
            controller.ViewBag.Countries = (await _countriesService.GetAllCountries()).Select(country =>
            {
                return new SelectListItem() { Text = country.CountryName, Value = country.CountryID.ToString() };
            });

            controller.ViewBag.Errors = controller
                .ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            context.Result = controller.View(context.ActionArguments.First().Value);
        }
        else
            await next();
    }
}
