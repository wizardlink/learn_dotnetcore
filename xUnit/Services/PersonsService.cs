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

    public PersonsService(bool initialize = true)
    {
        _personList = new List<Person>();
        _countriesService = new CountriesService();

        if (initialize)
        {
            _personList.AddRange(
                [
                    new Person
                    {
                        PersonID = Guid.Parse("9E5F189A-D9A3-4A93-811F-4CEB06672A6C"),
                        PersonName = "Aguste",
                        Email = "alledy@hello.com",
                        DateOfBirth = DateTime.Parse("1993-01-02"),
                        Gender = "Male",
                        Address = "Novick Terrace",
                        ReceiveNewsLetters = false,
                        CountryID = Guid.Parse("CCBE8E62-6081-4072-B36D-FA7987D50000"),
                    },
                    new Person
                    {
                        PersonID = Guid.Parse("0C4D0EF3-99A3-4FF7-97F4-E9E34296FA4C"),
                        PersonName = "Jasmina",
                        Email = "jasmina@hello.com",
                        DateOfBirth = DateTime.Parse("1991-06-24"),
                        Gender = "Female",
                        Address = "Fieldstone Lane",
                        ReceiveNewsLetters = false,
                        CountryID = Guid.Parse("CCBE8E62-6081-4072-B36D-FA7987D50000"),
                    },
                    new Person
                    {
                        PersonID = Guid.Parse("E983E477-1CAE-4567-8842-7C1F2928D6E2"),
                        PersonName = "Kendall",
                        Email = "kendall@hello.com",
                        DateOfBirth = DateTime.Parse("1993-08-13"),
                        Gender = "Male",
                        Address = "Pawling Alley",
                        ReceiveNewsLetters = false,
                        CountryID = Guid.Parse("CCBE8E62-6081-4072-B36D-FA7987D50000"),
                    },
                    new Person
                    {
                        PersonID = Guid.Parse("FFDFD955-98B5-4F04-A312-0F36A4C9B7C7"),
                        PersonName = "Kilian",
                        Email = "kilian@hello.com",
                        DateOfBirth = DateTime.Parse("1991-06-17"),
                        Gender = "Male",
                        Address = "Buhler Junction",
                        ReceiveNewsLetters = true,
                        CountryID = Guid.Parse("CCBE8E62-6081-4072-B36D-FA7987D50000"),
                    },
                    new Person
                    {
                        PersonID = Guid.Parse("CCBE8E62-6081-4072-B36D-FA7987D50000"),
                        PersonName = "Dulcinea",
                        Email = "dbus@hello.com",
                        DateOfBirth = DateTime.Parse("1996-09-02"),
                        Gender = "Female",
                        Address = "Sundown Point",
                        ReceiveNewsLetters = false,
                        CountryID = Guid.Parse("CCBE8E62-6081-4072-B36D-FA7987D50000"),
                    },
                ]
            );
        }
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

    public List<PersonResponse> GetFilteredPersons(string? searchBy, object? searchValue)
    {
        List<PersonResponse> allPersons = GetAllPersons();

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
