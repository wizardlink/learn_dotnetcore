using Entities;
using Microsoft.EntityFrameworkCore;
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
        _countriesService = new CountriesService(
            new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options)
        );
        _personService = new PersonsService(
            new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options),
            _countriesService
        );
        _testOutputHelper = testOutputHelper;
    }

    #region AddPerson
    [Fact]
    public async Task AddPerson_NullPerson()
    {
        PersonAddRequest? addRequest = null;

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
        {
            return _personService.AddPerson(addRequest);
        });
    }

    [Fact]
    public async Task AddPerson_NameIsNull()
    {
        PersonAddRequest addRequest = new PersonAddRequest() { PersonName = null };

        await Assert.ThrowsAsync<ArgumentException>(() =>
        {
            return _personService.AddPerson(addRequest);
        });
    }

    [Fact]
    public async Task AddPerson_ProperPersonDetails()
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

        PersonResponse response = await _personService.AddPerson(addRequest);

        List<PersonResponse> responseList = await _personService.GetAllPersons();

        Assert.True(response.PersonID != Guid.Empty);

        Assert.Contains(response, responseList);
    }
    #endregion

    #region GetPersonByID
    [Fact]
    public async Task GetPersonByID_NullPersonID()
    {
        Guid? personID = null;

        PersonResponse? response = await _personService.GetPersonById(personID);

        Assert.Null(response);
    }

    [Fact]
    public async Task GetPersonByID_WithPerson()
    {
        CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "Canada" };

        CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);

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

        PersonResponse response = await _personService.AddPerson(personRequest);

        PersonResponse? getPerson = await _personService.GetPersonById(response.PersonID);

        Assert.Equal(response, getPerson);
    }
    #endregion

    #region GetAllPersons
    [Fact]
    public async Task GetAllPersons_EmptyList()
    {
        List<PersonResponse> allPersons = await _personService.GetAllPersons();

        Assert.Empty(allPersons);
    }

    [Fact]
    public async Task GetAllPersons_AddFewPersons()
    {
        CountryAddRequest usaRequest = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest indiaRequest = new CountryAddRequest() { CountryName = "India" };

        CountryResponse usaResponse = await _countriesService.AddCountry(usaRequest);
        CountryResponse indiaResponse = await _countriesService.AddCountry(indiaRequest);

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

        var personResponses = new List<PersonResponse>();

        foreach (var person in new[] { personOne, personTwo })
        {
            personResponses.Add(await _personService.AddPerson(person));
        }

        _testOutputHelper.WriteLine("Expected:");
        personResponses.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        var allPersons = await _personService.GetAllPersons();

        _testOutputHelper.WriteLine("Got:");
        allPersons.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        personResponses.ForEach(person => Assert.Contains(person, allPersons));
    }
    #endregion

    #region GetFilteredPersons
    [Fact]
    public async Task GetFilteredPersons_EmptySearchText()
    {
        CountryAddRequest usaRequest = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest indiaRequest = new CountryAddRequest() { CountryName = "India" };

        CountryResponse usaResponse = await _countriesService.AddCountry(usaRequest);
        CountryResponse indiaResponse = await _countriesService.AddCountry(indiaRequest);

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

        var personResponses = new List<PersonResponse>();

        foreach (var person in new[] { personOne, personTwo })
        {
            personResponses.Add(await _personService.AddPerson(person));
        }

        _testOutputHelper.WriteLine("Expected:");
        personResponses.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        var allPersons = await _personService.GetFilteredPersons(nameof(Person.PersonName), string.Empty);

        _testOutputHelper.WriteLine("Got:");
        allPersons.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        personResponses.ForEach(person => Assert.Contains(person, allPersons));
    }

    [Fact]
    public async Task GetFilteredPersons_SearchByPersonName()
    {
        CountryAddRequest usaRequest = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest indiaRequest = new CountryAddRequest() { CountryName = "India" };

        CountryResponse usaResponse = await _countriesService.AddCountry(usaRequest);
        CountryResponse indiaResponse = await _countriesService.AddCountry(indiaRequest);

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

        var personResponses = new List<PersonResponse>();

        foreach (var person in new[] { personOne, personTwo })
        {
            personResponses.Add(await _personService.AddPerson(person));
        }

        _testOutputHelper.WriteLine("Expected:");
        personResponses.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        var allPersons = await _personService.GetFilteredPersons(nameof(PersonResponse.PersonName), "ma");

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
    public async Task GetSortedPersons()
    {
        CountryAddRequest usaRequest = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest indiaRequest = new CountryAddRequest() { CountryName = "India" };

        CountryResponse usaResponse = await _countriesService.AddCountry(usaRequest);
        CountryResponse indiaResponse = await _countriesService.AddCountry(indiaRequest);

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

        var personResponses = new List<PersonResponse>();

        foreach (var person in new[] { personOne, personTwo })
        {
            personResponses.Add(await _personService.AddPerson(person));
        }

        _testOutputHelper.WriteLine("Expected:");
        personResponses.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        var allPersons = await _personService.GetAllPersons();
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
    public async Task UpdatePerson_NullPerson()
    {
        PersonUpdateRequest? personUpdate = null;

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
        {
            return _personService.UpdatePerson(personUpdate);
        });
    }

    [Fact]
    public async Task UpdatePerson_InvalidPerson()
    {
        PersonUpdateRequest? updateRequest = new PersonUpdateRequest() { PersonID = Guid.NewGuid() };

        await Assert.ThrowsAsync<ArgumentException>(() =>
        {
            return _personService.UpdatePerson(updateRequest);
        });
    }

    [Fact]
    public async Task UpdatePerson_PersonNameIsNull()
    {
        var countryRequest = new CountryAddRequest() { CountryName = "UK" };

        var countryResponse = await _countriesService.AddCountry(countryRequest);

        PersonAddRequest personRequest = new PersonAddRequest()
        {
            PersonName = "John",
            CountryID = countryResponse.CountryID,
            Email = "abc@abc.com",
        };

        var personResponse = await _personService.AddPerson(personRequest);

        PersonUpdateRequest updateRequest = personResponse.ToPersonUpdateRequest();
        updateRequest.PersonName = null;

        await Assert.ThrowsAsync<ArgumentException>(() =>
        {
            return _personService.UpdatePerson(updateRequest);
        });
    }

    public async Task UpdatePerson_PersonFullDetails()
    {
        var countryRequest = new CountryAddRequest() { CountryName = "UK" };

        var countryResponse = await _countriesService.AddCountry(countryRequest);

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

        var personResponse = await _personService.AddPerson(personRequest);

        PersonUpdateRequest updateRequest = personResponse.ToPersonUpdateRequest();

        updateRequest.PersonName = "William";
        updateRequest.Email = "william@abc.com";

        var updateResponse = await _personService.UpdatePerson(updateRequest);
        var getPersonResponse = await _personService.GetPersonById(updateRequest.PersonID);

        Assert.Equal(updateResponse, getPersonResponse);
    }
    #endregion

    #region DeletePerson
    [Fact]
    public async Task DeletePerson_ValidPersonID()
    {
        CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "USA" };

        CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);

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

        var personResponse = await _personService.AddPerson(personRequest);

        bool isDeleted = await _personService.DeletePerson(personResponse.PersonID);

        Assert.True(isDeleted);
    }

    [Fact]
    public async Task DeletePerson_InvalidPersonID()
    {
        bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

        Assert.False(isDeleted);
    }
    #endregion
}
