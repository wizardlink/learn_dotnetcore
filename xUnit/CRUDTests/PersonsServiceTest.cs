using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryContracts;
using Serilog.Extensions.Hosting;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace CRUDTests;

public class PersonsServiceTest
{
    private readonly Fixture _fixture;

    private readonly Mock<IPersonsRepository> _mock;
    private readonly Mock<ILogger<PersonsService>> _mockLogger;
    private readonly Mock<DiagnosticContext> _mockDiagnosticContext;

    private readonly IPersonsRepository _repository;
    private readonly ILogger<PersonsService> _logger;
    private readonly DiagnosticContext _diagnosticContext;

    private readonly ICountriesService _countriesService;
    private readonly PersonsService _personService;

    private readonly ITestOutputHelper _testOutputHelper;

    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        _fixture = new Fixture();

        _mock = new();
        _mockLogger = new Mock<ILogger<PersonsService>>();
        _mockDiagnosticContext = new Mock<DiagnosticContext>();

        _repository = _mock.Object;
        _logger = _mockLogger.Object;
        _diagnosticContext = _mockDiagnosticContext.Object;

        DbContextMock<ApplicationDbContext> dbContextMock = new(
            new DbContextOptionsBuilder<ApplicationDbContext>().Options
        );

        ApplicationDbContext dbContext = dbContextMock.Object;

        dbContextMock.CreateDbSetMock(db => db.Countries, []);
        dbContextMock.CreateDbSetMock(db => db.Persons, []);

        _countriesService = new CountriesService(null);
        _personService = new PersonsService(_repository, _countriesService, _logger, _diagnosticContext);

        _testOutputHelper = testOutputHelper;
    }

    #region AddPerson
    [Fact]
    public async Task AddPerson_NullPerson()
    {
        PersonAddRequest? addRequest = null;

        Func<Task> action = (
            () =>
            {
                return _personService.AddPerson(addRequest);
            }
        );

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddPerson_NameIsNull()
    {
        PersonAddRequest addRequest = _fixture
            .Build<PersonAddRequest>()
            .Without(request => request.PersonName)
            .Create();

        Func<Task> action = () =>
        {
            return _personService.AddPerson(addRequest);
        };

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddPerson_FullPersonDetails_ToBeSuccessfull()
    {
        PersonAddRequest addRequest = _fixture
            .Build<PersonAddRequest>()
            .With(request => request.Email, "abc@abc.com")
            .Create();

        Person person = addRequest.ToPerson();

        _mock.Setup(repository => repository.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

        PersonResponse response = await _personService.AddPerson(addRequest);

        person.PersonID = response.PersonID;

        response.PersonID.Should().NotBeEmpty();

        response.Should().Be(person.ToPersonResponse());
    }
    #endregion

    #region GetPersonByID
    [Fact]
    public async Task GetPersonByID_NullPersonID()
    {
        Guid? personID = null;

        PersonResponse? response = await _personService.GetPersonById(personID);

        response.Should().BeNull();
    }

    [Fact]
    public async Task GetPersonByID_WithPerson()
    {
        Person? person = _fixture
            .Build<Person>()
            .With(model => model.Email, "abc@abc.com")
            .Without(model => model.Country)
            .Create();

        PersonResponse expectedResponse = person.ToPersonResponse();

        _mock.Setup(repository => repository.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);

        PersonResponse? getPerson = await _personService.GetPersonById(person.PersonID);

        getPerson.Should().Be(expectedResponse);
    }
    #endregion

    #region GetAllPersons
    [Fact]
    public async Task GetAllPersons_EmptyList()
    {
        _mock.Setup(repository => repository.GetAllPersons()).ReturnsAsync([]);

        List<PersonResponse> allPersons = await _personService.GetAllPersons();

        allPersons.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllPersons_AddFewPersons()
    {
        Person firstPerson = _fixture
            .Build<Person>()
            .With(model => model.Email, "abc@abc.com")
            .Without(model => model.Country)
            .Create();

        Person secondPerson = _fixture
            .Build<Person>()
            .With(model => model.Email, "abc@abc.com")
            .Without(model => model.Country)
            .Create();

        List<PersonResponse> personResponses = [firstPerson.ToPersonResponse(), secondPerson.ToPersonResponse()];

        _mock.Setup(repository => repository.GetAllPersons()).ReturnsAsync([firstPerson, secondPerson]);

        _testOutputHelper.WriteLine("Expected:");
        personResponses.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        var allPersons = await _personService.GetAllPersons();

        _testOutputHelper.WriteLine("Got:");
        allPersons.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        allPersons.Should().BeEquivalentTo(personResponses);
    }
    #endregion

    #region GetFilteredPersons
    [Fact]
    public async Task GetFilteredPersons_EmptySearchText()
    {
        Person firstPerson = _fixture
            .Build<Person>()
            .With(request => request.Email, "abc@abc.com")
            .Without(model => model.Country)
            .Create();

        Person secondPerson = _fixture
            .Build<Person>()
            .With(request => request.Email, "abc@abc.com")
            .Without(model => model.Country)
            .Create();

        List<PersonResponse> personResponses = [firstPerson.ToPersonResponse(), secondPerson.ToPersonResponse()];

        _mock.Setup(repository => repository.GetAllPersons()).ReturnsAsync([firstPerson, secondPerson]);

        _testOutputHelper.WriteLine("Expected:");
        personResponses.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        var allPersons = await _personService.GetFilteredPersons(nameof(Person.PersonName), string.Empty);

        _testOutputHelper.WriteLine("Got:");
        allPersons.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        allPersons.Should().BeEquivalentTo(personResponses);
    }

    [Fact]
    public async Task GetFilteredPersons_SearchByPersonName()
    {
        Person firstPerson = _fixture
            .Build<Person>()
            .With(request => request.Email, "abc@abc.com")
            .Without(model => model.Country)
            .Create();

        Person secondPerson = _fixture
            .Build<Person>()
            .With(request => request.Email, "abc@abc.com")
            .Without(model => model.Country)
            .Create();

        List<PersonResponse> personResponses = [firstPerson.ToPersonResponse(), secondPerson.ToPersonResponse()];

        _mock.Setup(repository => repository.GetAllPersons()).ReturnsAsync([firstPerson, secondPerson]);

        _testOutputHelper.WriteLine("Expected:");
        personResponses.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        var allPersons = await _personService.GetFilteredPersons(nameof(PersonResponse.PersonName), "ma");

        _testOutputHelper.WriteLine("Got:");
        allPersons.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        personResponses.ForEach(person =>
        {
            if (person.PersonName != null && person.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                allPersons.Should().Contain(person);
        });
    }
    #endregion

    #region GetSortedPersons
    [Fact]
    public async Task GetSortedPersons()
    {
        Person firstPerson = _fixture
            .Build<Person>()
            .With(request => request.Email, "abc@abc.com")
            .Without(model => model.Country)
            .Create();

        Person secondPerson = _fixture
            .Build<Person>()
            .With(request => request.Email, "abc@abc.com")
            .Without(model => model.Country)
            .Create();

        List<PersonResponse> personResponses = [firstPerson.ToPersonResponse(), secondPerson.ToPersonResponse()];

        _mock.Setup(repository => repository.GetAllPersons()).ReturnsAsync([firstPerson, secondPerson]);

        var allPersons = await _personService.GetAllPersons();
        var sortedPersons = _personService.GetSortedPersons(
            allPersons,
            nameof(PersonResponse.PersonName),
            SortOrderOptions.DESC
        );

        sortedPersons.ForEach(response => _testOutputHelper.WriteLine(response.ToString() + "\n"));

        sortedPersons.Should().BeInDescendingOrder(person => person.PersonName);
    }
    #endregion

    #region UpdatePerson
    [Fact]
    public async Task UpdatePerson_NullPerson()
    {
        PersonUpdateRequest? personUpdate = null;

        Func<Task> action = (
            () =>
            {
                return _personService.UpdatePerson(personUpdate);
            }
        );

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdatePerson_InvalidPerson()
    {
        PersonUpdateRequest? updateRequest = _fixture.Create<PersonUpdateRequest>();

        Func<Task> action = (
            () =>
            {
                return _personService.UpdatePerson(updateRequest);
            }
        );

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdatePerson_PersonNameIsNull()
    {
        Person person = _fixture
            .Build<Person>()
            .With(model => model.Email, "abc@abc.com")
            .With(model => model.Gender, GenderOptions.Male.ToString())
            .Without(model => model.Country)
            .Without(model => model.PersonName)
            .Create();

        var personResponse = person.ToPersonResponse();

        PersonUpdateRequest updateRequest = personResponse.ToPersonUpdateRequest();

        Func<Task> action = (
            () =>
            {
                return _personService.UpdatePerson(updateRequest);
            }
        );

        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdatePerson_PersonFullDetails()
    {
        Person person = _fixture
            .Build<Person>()
            .With(model => model.Email, "abc@abc.com")
            .With(model => model.Gender, GenderOptions.Male.ToString())
            .Without(model => model.Country)
            .Create();

        var personResponse = person.ToPersonResponse();

        PersonUpdateRequest updateRequest = personResponse.ToPersonUpdateRequest();

        _mock.Setup(repository => repository.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person);
        _mock.Setup(repository => repository.GetPersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(person);

        var updateResponse = await _personService.UpdatePerson(updateRequest);

        updateResponse.Should().Be(personResponse);
    }
    #endregion

    #region DeletePerson
    [Fact]
    public async Task DeletePerson_ValidPersonID()
    {
        Person person = _fixture
            .Build<Person>()
            .With(model => model.Email, "abc@abc.com")
            .Without(model => model.Country)
            .Create();

        _mock.Setup(repository => repository.DeletePersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(true);

        bool isDeleted = await _personService.DeletePerson(person.PersonID);

        isDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task DeletePerson_InvalidPersonID()
    {
        _mock.Setup(repository => repository.DeletePersonByPersonID(It.IsAny<Guid>())).ReturnsAsync(false);

        bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

        isDeleted.Should().BeFalse();
    }
    #endregion
}
