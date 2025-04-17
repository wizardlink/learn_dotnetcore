using System.Reflection;
using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using SerilogTimings;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class PersonsService : IPersonService
{
    private readonly IPersonsRepository _repository;
    private readonly ICountriesService _countriesService;
    private readonly ILogger<PersonsService> _logger;
    private readonly IDiagnosticContext _diagnosticContext;

    public PersonsService(
        IPersonsRepository personsRepository,
        ICountriesService countriesService,
        ILogger<PersonsService> logger,
        IDiagnosticContext diagnosticContext
    )
    {
        _repository = personsRepository;
        _countriesService = countriesService;
        _logger = logger;
        _diagnosticContext = diagnosticContext;
    }

    public async Task<PersonResponse> AddPerson(PersonAddRequest? addRequest)
    {
        if (addRequest == null)
            throw new ArgumentNullException(nameof(addRequest));

        ValidationHelper.ModelValidation(addRequest);

        Person person = addRequest.ToPerson();

        person.PersonID = Guid.NewGuid();

        await _repository.AddPerson(person);

        return person.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetAllPersons()
    {
        _logger.LogInformation("GetALlPersons of PersonsService");
        return [.. (await _repository.GetAllPersons()).Select(person => person.ToPersonResponse())];
    }

    public async Task<PersonResponse?> GetPersonById(Guid? personID)
    {
        if (!personID.HasValue)
            return null;

        Person? person = await _repository.GetPersonByPersonID(personID.Value);

        if (person == null)
            return null;

        return person.ToPersonResponse();
    }

    public async Task<List<PersonResponse>> GetFilteredPersons(string? searchBy, object? searchValue)
    {
        _logger.LogInformation("GetFilteredPersons of PersonsService");
        List<PersonResponse> filteredPeople = [];

        using (Operation.Time("Time for GetFilteredPersons"))
        {
            List<PersonResponse> allPersons = await GetAllPersons();

            if (string.IsNullOrEmpty(searchBy))
                return allPersons;

            var propertyToSearch =
                typeof(PersonResponse).GetTypeInfo().GetProperty(searchBy)
                ?? throw new ArgumentException("searchBy does not matches any fields from type PersonResponse");

            filteredPeople =
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

        _diagnosticContext.Set("People", filteredPeople);

        return filteredPeople;
    }

    public List<PersonResponse> GetSortedPersons(
        List<PersonResponse> allPersons,
        string sortBy,
        SortOrderOptions sortOptions
    )
    {
        _logger.LogInformation("GetSortedPersons of PersonsService");

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

        var person = await _repository.GetPersonByPersonID(request.PersonID);

        if (person == null)
            throw new ArgumentException("Given person ID does not exists");

        person.PersonName = request.PersonName;
        person.Email = request.Email;
        person.ReceiveNewsLetters = request.ReceiveNewsLetters;
        person.DateOfBirth = request.DateOfBirth;
        person.Address = request.Address;
        person.Gender = request.Gender.ToString();

        await _repository.UpdatePerson(person);

        return person.ToPersonResponse();
    }

    public async Task<bool> DeletePerson(Guid? personID)
    {
        if (!personID.HasValue)
            throw new ArgumentNullException(nameof(personID));

        return await _repository.DeletePersonByPersonID(personID.Value);
    }
}
