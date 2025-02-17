using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers;

[Route("persons")]
public class PersonsController : Controller
{
    private readonly IPersonService _personService;
    private readonly ICountriesService _countriesService;

    public PersonsController(IPersonService personService, ICountriesService countriesService)
    {
        _personService = personService;
        _countriesService = countriesService;
    }

    [Route("index")]
    [Route("/")]
    public ActionResult Index(
        [FromQuery] string? searchBy,
        [FromQuery] string? searchString,
        [FromQuery] string sortBy = nameof(PersonResponse.PersonName),
        [FromQuery] SortOrderOptions sortOrder = SortOrderOptions.ASC
    )
    {
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

        var persons = _personService.GetFilteredPersons(searchBy, searchString);
        #endregion

        #region Sort
        persons = _personService.GetSortedPersons(persons, sortBy, sortOrder);
        #endregion

        // Preserve inputs
        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;
        ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentSortOrder = sortOrder;

        return View(persons);
    }

    [Route("create")]
    [HttpGet]
    public ActionResult Create()
    {
        ViewBag.Countries = _countriesService
            .GetAllCountries()
            .Select(country =>
            {
                return new SelectListItem() { Text = country.CountryName, Value = country.CountryID.ToString() };
            });

        return View();
    }

    [Route("create")]
    [HttpPost]
    public ActionResult Create(PersonAddRequest personAddRequest)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Countries = _countriesService
                .GetAllCountries()
                .Select(country =>
                {
                    return new SelectListItem() { Text = country.CountryName, Value = country.CountryID.ToString() };
                });
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).SelectMany(e => e.ErrorMessage).ToList();
            return View();
        }

        _personService.AddPerson(personAddRequest);

        return RedirectToAction("Index", "Persons");
    }

    [HttpGet]
    [Route("[action]/{personId}")]
    public ActionResult Edit(Guid personId)
    {
        var person = _personService.GetPersonById(personId);

        if (person == null)
            return RedirectToAction("Index");

        ViewBag.Countries = _countriesService
            .GetAllCountries()
            .Select(country =>
            {
                return new SelectListItem() { Text = country.CountryName, Value = country.CountryID.ToString() };
            });

        PersonUpdateRequest updateRequest = person.ToPersonUpdateRequest();
        return View(updateRequest);
    }

    [HttpPost]
    [Route("[action]/{personId}")]
    public ActionResult Edit(Guid personId, PersonUpdateRequest updateRequest)
    {
        var person = _personService.GetPersonById(updateRequest.PersonID);
        if (person == null)
            return RedirectToAction("Index");

        if (ModelState.IsValid)
        {
            _personService.UpdatePerson(updateRequest);
            return RedirectToAction("Index");
        }
        else
        {
            ViewBag.Countries = _countriesService
                .GetAllCountries()
                .Select(country =>
                {
                    return new SelectListItem() { Text = country.CountryName, Value = country.CountryID.ToString() };
                });
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).SelectMany(e => e.ErrorMessage).ToList();
            return View(person.ToPersonUpdateRequest());
        }
    }

    [HttpGet]
    [Route("[action]/{personId}")]
    public ActionResult Delete(Guid personId)
    {
        var person = _personService.GetPersonById(personId);
        if (person == null)
            return RedirectToAction("Index");

        return View(person);
    }

    [HttpPost]
    [Route("[action]/{personId}")]
    public ActionResult Delete(Guid personId, PersonUpdateRequest updateRequest)
    {
        var person = _personService.GetPersonById(updateRequest.PersonID);

        if (person != null)
            _personService.DeletePerson(updateRequest.PersonID);

        return RedirectToAction("Index");
    }
}
