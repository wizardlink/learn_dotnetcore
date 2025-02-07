using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace CRUDTests;

public class PersonsServiceTest
{
    private readonly IPersonService _personService;
    private readonly ICountriesService _countriesService;
    private readonly ITestOutputHelper _testOutputHelper;

    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        _personService = new PersonsService();
        _countriesService = new CountriesService();
        _testOutputHelper = testOutputHelper;
    }

    #region AddPerson
    [Fact]
    public void AddPerson_NullPerson()
    {
        PersonAddRequest? addRequest = null;

        Assert.Throws<ArgumentNullException>(() =>
        {
            _personService.AddPerson(addRequest);
        });
    }

    [Fact]
    public void AddPerson_NameIsNull()
    {
        PersonAddRequest addRequest = new PersonAddRequest() { PersonName = null };

        Assert.Throws<ArgumentException>(() =>
        {
            _personService.AddPerson(addRequest);
        });
    }

    [Fact]
    public void AddPerson_ProperPersonDetails()
    {
        PersonAddRequest addRequest = new PersonAddRequest()
        {
            PersonName = "John",
            Email = "abc@abc.com",
            Address = "Address",
            CountryID = Guid.NewGuid(),
            Gender = GenderOptions.Other,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            ReceiveNewsLetters = true,
        };

        PersonResponse response = _personService.AddPerson(addRequest);

        List<PersonResponse> responseList = _personService.GetAllPersons();

        Assert.True(response.PersonID != Guid.Empty);

        Assert.Contains(response, responseList);
    }
    #endregion

    #region GetPersonByID
    [Fact]
    public void GetPersonByID_NullPersonID()
    {
        Guid? personID = null;

        PersonResponse? response = _personService.GetPersonById(personID);

        Assert.Null(response);
    }

    [Fact]
    public void GetPersonByID_WithPerson()
    {
        CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "Canada" };

        CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);

        PersonAddRequest personRequest = new PersonAddRequest()
        {
            PersonName = "John",
            Email = "abc@abc.com",
            Address = "Address",
            CountryID = countryResponse.CountryID,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Gender = GenderOptions.Other,
            ReceiveNewsLetters = false,
        };

        PersonResponse response = _personService.AddPerson(personRequest);

        PersonResponse? getPerson = _personService.GetPersonById(response.PersonID);

        Assert.Equal(response, getPerson);
    }
    #endregion

    #region GetAllPersons
    [Fact]
    public void GetAllPersons_EmptyList()
    {
        List<PersonResponse> allPersons = _personService.GetAllPersons();

        Assert.Empty(allPersons);
    }

    [Fact]
    public void GetAllPersons_AddFewPersons()
    {
        CountryAddRequest usaRequest = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest indiaRequest = new CountryAddRequest() { CountryName = "India" };

        CountryResponse usaResponse = _countriesService.AddCountry(usaRequest);
        CountryResponse indiaResponse = _countriesService.AddCountry(indiaRequest);

        PersonAddRequest personOne = new PersonAddRequest()
        {
            PersonName = "John",
            Email = "abc@abc.com",
            Address = "Address",
            CountryID = usaResponse.CountryID,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Gender = GenderOptions.Other,
            ReceiveNewsLetters = false,
        };

        PersonAddRequest personTwo = new PersonAddRequest()
        {
            PersonName = "Maria",
            Email = "abc@abc.com",
            Address = "Address",
            CountryID = indiaResponse.CountryID,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Gender = GenderOptions.Other,
            ReceiveNewsLetters = false,
        };

        var personRequests = new List<PersonAddRequest>() { personOne, personTwo };
        var personResponses = new List<PersonResponse>();

        personRequests.ForEach(person => personResponses.Add(_personService.AddPerson(person)));

        _testOutputHelper.WriteLine("Expected:");
        personResponses.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        var allPersons = _personService.GetAllPersons();

        _testOutputHelper.WriteLine("Got:");
        allPersons.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        personResponses.ForEach(person => Assert.Contains(person, allPersons));
    }
    #endregion

    #region GetFilteredPersons
    [Fact]
    public void GetFilteredPersons_EmptySearchText()
    {
        CountryAddRequest usaRequest = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest indiaRequest = new CountryAddRequest() { CountryName = "India" };

        CountryResponse usaResponse = _countriesService.AddCountry(usaRequest);
        CountryResponse indiaResponse = _countriesService.AddCountry(indiaRequest);

        PersonAddRequest personOne = new PersonAddRequest()
        {
            PersonName = "John",
            Email = "abc@abc.com",
            Address = "Address",
            CountryID = usaResponse.CountryID,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Gender = GenderOptions.Other,
            ReceiveNewsLetters = false,
        };

        PersonAddRequest personTwo = new PersonAddRequest()
        {
            PersonName = "Maria",
            Email = "abc@abc.com",
            Address = "Address",
            CountryID = indiaResponse.CountryID,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Gender = GenderOptions.Other,
            ReceiveNewsLetters = false,
        };

        var personRequests = new List<PersonAddRequest>() { personOne, personTwo };
        var personResponses = new List<PersonResponse>();

        personRequests.ForEach(person => personResponses.Add(_personService.AddPerson(person)));

        _testOutputHelper.WriteLine("Expected:");
        personResponses.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        var allPersons = _personService.GetFilteredPersons(nameof(Person.PersonName), string.Empty);

        _testOutputHelper.WriteLine("Got:");
        allPersons.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        personResponses.ForEach(person => Assert.Contains(person, allPersons));
    }

    [Fact]
    public void GetFilteredPersons_SearchByPersonName()
    {
        CountryAddRequest usaRequest = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest indiaRequest = new CountryAddRequest() { CountryName = "India" };

        CountryResponse usaResponse = _countriesService.AddCountry(usaRequest);
        CountryResponse indiaResponse = _countriesService.AddCountry(indiaRequest);

        PersonAddRequest personOne = new PersonAddRequest()
        {
            PersonName = "Oromayer",
            Email = "abc@abc.com",
            Address = "Address",
            CountryID = usaResponse.CountryID,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Gender = GenderOptions.Other,
            ReceiveNewsLetters = false,
        };

        PersonAddRequest personTwo = new PersonAddRequest()
        {
            PersonName = "Maria",
            Email = "abc@abc.com",
            Address = "Address",
            CountryID = indiaResponse.CountryID,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Gender = GenderOptions.Other,
            ReceiveNewsLetters = false,
        };

        var personRequests = new List<PersonAddRequest>() { personOne, personTwo };
        var personResponses = new List<PersonResponse>();

        personRequests.ForEach(person => personResponses.Add(_personService.AddPerson(person)));

        _testOutputHelper.WriteLine("Expected:");
        personResponses.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        var allPersons = _personService.GetFilteredPersons(nameof(PersonResponse.PersonName), "ma");

        _testOutputHelper.WriteLine("Got:");
        allPersons.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        personResponses.ForEach(person =>
        {
            if (person.PersonName != null && person.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                Assert.Contains(person, allPersons);
        });
    }
    #endregion

    #region GetSortedPersons
    [Fact]
    public void GetSortedPersons()
    {
        CountryAddRequest usaRequest = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest indiaRequest = new CountryAddRequest() { CountryName = "India" };

        CountryResponse usaResponse = _countriesService.AddCountry(usaRequest);
        CountryResponse indiaResponse = _countriesService.AddCountry(indiaRequest);

        PersonAddRequest personOne = new PersonAddRequest()
        {
            PersonName = "Oromayer",
            Email = "abc@abc.com",
            Address = "Address",
            CountryID = usaResponse.CountryID,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Gender = GenderOptions.Other,
            ReceiveNewsLetters = false,
        };

        PersonAddRequest personTwo = new PersonAddRequest()
        {
            PersonName = "Maria",
            Email = "abc@abc.com",
            Address = "Address",
            CountryID = indiaResponse.CountryID,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Gender = GenderOptions.Other,
            ReceiveNewsLetters = false,
        };

        var personRequests = new List<PersonAddRequest>() { personOne, personTwo };
        var personResponses = new List<PersonResponse>();

        personRequests.ForEach(person => personResponses.Add(_personService.AddPerson(person)));

        _testOutputHelper.WriteLine("Expected:");
        personResponses.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        var allPersons = _personService.GetAllPersons();
        var sortedPersons = _personService.GetSortedPersons(
            allPersons,
            nameof(PersonResponse.PersonName),
            SortOrderOptions.DESC
        );

        _testOutputHelper.WriteLine("Got:");
        sortedPersons.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        // Order the list to compare
        personResponses = personResponses.OrderByDescending(person => person.PersonName).ToList();

        for (int i = 0; i < personResponses.Count; i++)
        {
            Assert.Equal(personResponses[i], sortedPersons[i]);
        }
    }
    #endregion

    #region UpdatePerson
    [Fact]
    public void UpdatePerson_NullPerson()
    {
        PersonUpdateRequest? personUpdate = null;

        Assert.Throws<ArgumentNullException>(() =>
        {
            _personService.UpdatePerson(personUpdate);
        });
    }

    [Fact]
    public void UpdatePerson_InvalidPerson()
    {
        PersonUpdateRequest? updateRequest = new PersonUpdateRequest() { PersonID = Guid.NewGuid() };

        Assert.Throws<ArgumentException>(() =>
        {
            _personService.UpdatePerson(updateRequest);
        });
    }

    [Fact]
    public void UpdatePerson_PersonNameIsNull()
    {
        var countryRequest = new CountryAddRequest() { CountryName = "UK" };

        var countryResponse = _countriesService.AddCountry(countryRequest);

        PersonAddRequest personRequest = new PersonAddRequest()
        {
            PersonName = "John",
            CountryID = countryResponse.CountryID,
            Email = "abc@abc.com",
        };

        var personResponse = _personService.AddPerson(personRequest);

        PersonUpdateRequest updateRequest = personResponse.ToPersonUpdateRequest();
        updateRequest.PersonName = null;

        Assert.Throws<ArgumentException>(() =>
        {
            _personService.UpdatePerson(updateRequest);
        });
    }

    public void UpdatePerson_PersonFullDetails()
    {
        var countryRequest = new CountryAddRequest() { CountryName = "UK" };

        var countryResponse = _countriesService.AddCountry(countryRequest);

        PersonAddRequest personRequest = new PersonAddRequest()
        {
            PersonName = "John",
            CountryID = countryResponse.CountryID,
            Address = "Abc",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "abc@abc.com",
            Gender = GenderOptions.Other,
            ReceiveNewsLetters = true,
        };

        var personResponse = _personService.AddPerson(personRequest);

        PersonUpdateRequest updateRequest = personResponse.ToPersonUpdateRequest();

        updateRequest.PersonName = "William";
        updateRequest.Email = "william@abc.com";

        var updateResponse = _personService.UpdatePerson(updateRequest);
        var getPersonResponse = _personService.GetPersonById(updateRequest.PersonID);

        Assert.Equal(updateResponse, getPersonResponse);
    }
    #endregion

    #region DeletePerson
    [Fact]
    public void DeletePerson_ValidPersonID()
    {
        CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "USA" };

        CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);

        PersonAddRequest personRequest = new PersonAddRequest()
        {
            PersonName = "John",
            CountryID = countryResponse.CountryID,
            Address = "Abc",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "abc@abc.com",
            Gender = GenderOptions.Other,
            ReceiveNewsLetters = true,
        };

        var personResponse = _personService.AddPerson(personRequest);

        bool isDeleted = _personService.DeletePerson(personResponse.PersonID);

        Assert.True(isDeleted);
    }

    [Fact]
    public void DeletePerson_InvalidPersonID()
    {
        bool isDeleted = _personService.DeletePerson(Guid.NewGuid());

        Assert.False(isDeleted);
    }
    #endregion
}
