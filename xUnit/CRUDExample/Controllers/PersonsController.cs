using CRUDExample.Filters;
using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.AuthorizationFilters;
using CRUDExample.Filters.ExceptionFilters;
using CRUDExample.Filters.ResourceFilters;
using CRUDExample.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers;

[Route("persons")]
// [TypeFilter<HandleExceptionFilter>()]
[ResponseHeaderFilterFactory("My-Key-From-Controller", "My-Value-From-Controller")]
public class PersonsController : Controller
{
    private readonly IPersonService _personService;
    private readonly ICountriesService _countriesService;
    private readonly ILogger<PersonsController> _logger;

    public PersonsController(
        IPersonService personService,
        ICountriesService countriesService,
        ILogger<PersonsController> logger
    )
    {
        _personService = personService;
        _countriesService = countriesService;
        _logger = logger;
    }

    [Route("index")]
    [Route("/")]
    [ServiceFilter(typeof(PersonsListActionFilter))]
    [ResponseHeaderFilterFactory("X-Custom-Key", "Custom-Value")]
    [TypeFilter<PersonsListResultFilter>()]
    [SkipFilter]
    public async Task<ActionResult> Index(
        [FromQuery] string? searchBy,
        [FromQuery] string? searchString,
        [FromQuery] string sortBy = nameof(PersonResponse.PersonName),
        [FromQuery] SortOrderOptions sortOrder = SortOrderOptions.ASC
    )
    {
        _logger.LogInformation("Index action");

        _logger.LogDebug(
            $"searchBy: {searchBy}, searchString: {searchString}, sortBy: {sortBy}, sortOrder: {sortOrder}"
        );

        #region Search
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            { nameof(PersonResponse.PersonName), "PersonName" },
            { nameof(PersonResponse.Email), "Email" },
            { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
            { nameof(PersonResponse.Gender), "Gender" },
            { nameof(PersonResponse.CountryID), "Country" },
            { nameof(PersonResponse.Address), "Address" },
        };

        var persons = await _personService.GetFilteredPersons(searchBy, searchString);
        #endregion

        #region Sort
        persons = _personService.GetSortedPersons(persons, sortBy, sortOrder);
        #endregion

        throw new Exception("Hahahaha");

        return View(persons);
    }

    [Route("create")]
    [HttpGet]
    public async Task<ActionResult> Create()
    {
        ViewBag.Countries = (await _countriesService.GetAllCountries()).Select(country =>
        {
            return new SelectListItem() { Text = country.CountryName, Value = country.CountryID.ToString() };
        });

        return View();
    }

    [Route("create")]
    [HttpPost]
    [TypeFilter<PersonCreateAndEditPostActionFilter>()]
    [TypeFilter<FeatureDisabledResourceFilter>(Arguments = new object[] { false })]
    public async Task<ActionResult> Create(PersonAddRequest personAddRequest)
    {
        await _personService.AddPerson(personAddRequest);

        return RedirectToAction("Index", "Persons");
    }

    [HttpGet]
    [Route("[action]/{personId}")]
    [TypeFilter<TokenResultFilter>()]
    public async Task<ActionResult> Edit(Guid personId)
    {
        var person = await _personService.GetPersonById(personId);

        if (person == null)
            return RedirectToAction("Index");

        ViewBag.Countries = (await _countriesService.GetAllCountries()).Select(country =>
        {
            return new SelectListItem() { Text = country.CountryName, Value = country.CountryID.ToString() };
        });

        PersonUpdateRequest updateRequest = person.ToPersonUpdateRequest();
        return View(updateRequest);
    }

    [HttpPost]
    [Route("[action]/{personId}")]
    [TypeFilter<TokenAuthorizationFilter>()]
    [TypeFilter<PersonAlwaysRunResultFilter>()]
    public async Task<ActionResult> Edit(Guid personId, PersonUpdateRequest updateRequest)
    {
        var person = await _personService.GetPersonById(updateRequest.PersonID);
        if (person == null)
            return RedirectToAction("Index");

        if (ModelState.IsValid)
        {
            await _personService.UpdatePerson(updateRequest);
            return RedirectToAction("Index");
        }
        else
        {
            ViewBag.Countries = (await _countriesService.GetAllCountries()).Select(country =>
            {
                return new SelectListItem() { Text = country.CountryName, Value = country.CountryID.ToString() };
            });
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).SelectMany(e => e.ErrorMessage).ToList();
            return View(person.ToPersonUpdateRequest());
        }
    }

    [HttpGet]
    [Route("[action]/{personId}")]
    public async Task<ActionResult> Delete(Guid personId)
    {
        var person = await _personService.GetPersonById(personId);
        if (person == null)
            return RedirectToAction("Index");

        return View(person);
    }

    [HttpPost]
    [Route("[action]/{personId}")]
    public async Task<ActionResult> Delete(Guid personId, PersonUpdateRequest updateRequest)
    {
        var person = await _personService.GetPersonById(updateRequest.PersonID);

        if (person != null)
            await _personService.DeletePerson(updateRequest.PersonID);

        return RedirectToAction("Index");
    }
}
