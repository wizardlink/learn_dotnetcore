using System.Reflection;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class PersonsService : IPersonService
{
    private readonly PersonsDbContext _database;
    private readonly ICountriesService _countriesService;

    public PersonsService(PersonsDbContext personsDbContext, ICountriesService countriesService)
    {
        _database = personsDbContext;
        _countriesService = countriesService;
    }

    public async Task<PersonResponse> AddPerson(PersonAddRequest? addRequest)
    {
        if (addRequest == null)
            throw new ArgumentNullException(nameof(addRequest));

        ValidationHelper.ModelValidation(addRequest);

        Person person = addRequest.ToPerson();

        person.PersonID = Guid.NewGuid();

        _database.Persons.Add(person);
        await _database.SaveChangesAsync();

        return person.ToPersonResponse();
    }

    public Task<List<PersonResponse>> GetAllPersons()
    {
        return _database.Persons.Include("Country").Select(person => person.ToPersonResponse()).ToListAsync();
    }

    public async Task<PersonResponse?> GetPersonById(Guid? personID)
    {
        if (personID == null)
            return null;

        Person? person = await _database
            .Persons.Include("Country")
            .FirstOrDefaultAsync(person => person.PersonID == personID);

        if (person == null)
            return null;

        return person.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetFilteredPersons(string? searchBy, object? searchValue)
    {
        List<PersonResponse> allPersons = await GetAllPersons();

        if (string.IsNullOrEmpty(searchBy))
            return allPersons;

        var propertyToSearch = typeof(PersonResponse).GetTypeInfo().GetProperty(searchBy);

        if (propertyToSearch == null)
            throw new ArgumentException("searchBy does not matches any fields from type PersonResponse");

        return
        [
            .. allPersons.Where(person =>
            {
                var fieldValue = propertyToSearch.GetValue(person);

                if (fieldValue == null)
                    return false;

                bool defaultCompare() => fieldValue == searchValue;

                return fieldValue switch
                {
                    string fieldString => searchValue is string searchString
                        ? fieldString.Contains(searchString)
                        : defaultCompare(),

                    DateTime fieldDate => searchValue is string searchString
                        ? fieldDate.Date.CompareTo(DateTime.Parse(searchString)) == 0
                        : defaultCompare(),

                    _ => defaultCompare(),
                };
            }),
        ];
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

    public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        ValidationHelper.ModelValidation(request);

        var person = await _database.Persons.FirstOrDefaultAsync(person => person.PersonID == request.PersonID);

        if (person == null)
            throw new ArgumentException("Given person ID does not exists");

        person.PersonName = request.PersonName;
        person.Email = request.Email;
        person.ReceiveNewsLetters = request.ReceiveNewsLetters;
        person.DateOfBirth = request.DateOfBirth;
        person.Address = request.Address;
        person.Gender = request.Gender.ToString();

        await _database.SaveChangesAsync();

        return person.ToPersonResponse();
    }

    public async Task<bool> DeletePerson(Guid? personID)
    {
        if (personID == null)
            throw new ArgumentNullException(nameof(personID));

        var person = await GetPersonById(personID);
        if (person == null)
            return false;

        _database.Persons.Remove(_database.Persons.First(entry => entry.PersonID == personID));
        _database.SaveChanges();

        return true;
    }
}
