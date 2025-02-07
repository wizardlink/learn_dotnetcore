using System.Reflection;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class PersonsService : IPersonService
{
    private readonly List<Person> _personList;
    private readonly ICountriesService _countriesService;

    public PersonsService()
    {
        _personList = new List<Person>();
        _countriesService = new CountriesService();
    }

    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        PersonResponse response = person.ToPersonResponse();

        response.Country = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;

        return response;
    }

    public PersonResponse AddPerson(PersonAddRequest? addRequest)
    {
        if (addRequest == null)
            throw new ArgumentNullException(nameof(addRequest));

        ValidationHelper.ModelValidation(addRequest);

        Person person = addRequest.ToPerson();

        person.PersonID = Guid.NewGuid();

        _personList.Add(person);

        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetAllPersons()
    {
        return _personList.Select(person => person.ToPersonResponse()).ToList();
    }

    public PersonResponse? GetPersonById(Guid? personID)
    {
        if (personID == null)
            return null;

        Person? person = _personList.FirstOrDefault(person => person.PersonID == personID);

        if (person == null)
            return null;

        return person.ToPersonResponse();
    }

    public List<PersonResponse> GetFilteredPersons(string searchBy, object? searchValue)
    {
        List<PersonResponse> allPersons = GetAllPersons();

        if (string.IsNullOrEmpty(searchBy))
            return allPersons;

        var propertyToSearch = typeof(PersonResponse).GetTypeInfo().GetProperty(searchBy);

        if (propertyToSearch == null)
            throw new ArgumentException("searchBy does not matches any fields from type PersonResponse");

        return allPersons
            .Where(person =>
            {
                var fieldValue = propertyToSearch.GetValue(person);

                if (fieldValue == null)
                    return false;

                if (fieldValue is String && searchValue is String)
                    return ((String)fieldValue).Contains((String)searchValue, StringComparison.OrdinalIgnoreCase);
                else
                    return fieldValue == searchValue;
            })
            .ToList();
    }

    public List<PersonResponse> GetSortedPersons(
        List<PersonResponse> allPersons,
        string sortBy,
        SortOrderOptions sortOptions
    )
    {
        if (string.IsNullOrEmpty(sortBy))
            return allPersons;

        var propertyToSort = typeof(PersonResponse).GetTypeInfo().GetProperty(sortBy);

        if (propertyToSort == null)
            throw new ArgumentException("sortBy does not matches any fields from type PersonResponse");

        object? comparer = null;
        Func<PersonResponse, object?>? keySelector = (person) =>
        {
            var propertyValue = propertyToSort.GetValue(person);

            if (propertyValue is String)
                comparer = StringComparer.OrdinalIgnoreCase;

            return propertyValue;
        };

        return sortOptions == SortOrderOptions.ASC
            ? allPersons.OrderBy(keySelector, comparer as IComparer<object?>).ToList()
            : allPersons.OrderByDescending(keySelector, comparer as IComparer<object?>).ToList();
    }

    public PersonResponse UpdatePerson(PersonUpdateRequest? request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        ValidationHelper.ModelValidation(request);

        var person = _personList.FirstOrDefault(person => person.PersonID == request.PersonID);

        if (person == null)
            throw new ArgumentException("Given person ID does not exists");

        person.PersonName = request.PersonName;
        person.Email = request.Email;
        person.ReceiveNewsLetters = request.ReceiveNewsLetters;
        person.DateOfBirth = request.DateOfBirth;
        person.Address = request.Address;
        person.Gender = request.Gender.ToString();

        return person.ToPersonResponse();
    }

    public bool DeletePerson(Guid? personID)
    {
        if (personID == null)
            throw new ArgumentNullException(nameof(personID));

        int listCount = _personList.Count;
        bool test = object.ReferenceEquals(listCount, _personList.Count);

        _personList.RemoveAll(person => person.PersonID == personID);

        if (listCount > _personList.Count)
            return true;
        else
            return false;
    }
}
