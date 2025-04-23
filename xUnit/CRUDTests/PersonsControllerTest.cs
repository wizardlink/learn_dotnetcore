using AutoFixture;
using CRUDExample.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDTests;

public class PersonsControllerTest
{
    private readonly Fixture _fixture;

    private readonly Mock<IPersonService> _mockPersons;
    private readonly Mock<ICountriesService> _mockCountries;
    private readonly Mock<ILogger<PersonsController>> _mockLogger;

    private readonly IPersonService _personsService;
    private readonly ICountriesService _countriesService;
    private readonly ILogger<PersonsController> _logger;

    public PersonsControllerTest()
    {
        _fixture = new Fixture();

        _mockPersons = new Mock<IPersonService>();
        _mockCountries = new Mock<ICountriesService>();
        _mockLogger = new Mock<ILogger<PersonsController>>();

        _personsService = _mockPersons.Object;
        _countriesService = _mockCountries.Object;
        _logger = _mockLogger.Object;
    }

    #region Index
    [Fact]
    public async Task Index_ShouldReturnIndexViewWithPersonsList()
    {
        List<PersonResponse> personResponses = _fixture.Create<List<PersonResponse>>();

        PersonsController personsController = new(_personsService, _countriesService, _logger);

        _mockPersons
            .Setup(service => service.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(personResponses);

        _mockPersons
            .Setup(service =>
                service.GetSortedPersons(
                    It.IsAny<List<PersonResponse>>(),
                    It.IsAny<string>(),
                    It.IsAny<SortOrderOptions>()
                )
            )
            .Returns(personResponses);

        var actionResult = await personsController.Index(
            _fixture.Create<string>(),
            _fixture.Create<string>(),
            _fixture.Create<string>(),
            _fixture.Create<SortOrderOptions>()
        );

        actionResult.Should().BeOfType<ViewResult>();

        var viewData = ((ViewResult)actionResult).ViewData;

        viewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
        viewData.Model.Should().Be(personResponses);
    }
    #endregion

    #region Create
    [Fact]
    public async Task Create_IfModelErrors_RetunCreateView()
    {
        var addRequest = _fixture.Create<PersonAddRequest>();

        var response = _fixture.Create<PersonResponse>();

        var countryResponses = _fixture.Create<List<CountryResponse>>();

        _mockCountries.Setup(service => service.GetAllCountries()).ReturnsAsync(countryResponses);

        PersonsController personsController = new(_personsService, _countriesService, _logger);

        // Forcibly create a model error
        personsController.ModelState.AddModelError("PersonName", "Person Name cannot be blank.");

        var actionResult = await personsController.Create(addRequest);

        actionResult.Should().BeOfType<RedirectToActionResult>();

        var viewData = ((ViewResult)actionResult).ViewData;

        viewData.Model.Should().BeAssignableTo<PersonAddRequest>();
        viewData.Model.Should().Be(addRequest);
    }

    [Fact]
    public async Task Create_IfNoModelErrors_ReturnIndexView()
    {
        var addRequest = _fixture.Create<PersonAddRequest>();

        var response = _fixture.Create<PersonResponse>();

        var countryResponses = _fixture.Create<List<CountryResponse>>();

        _mockPersons.Setup(service => service.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(response);

        PersonsController personsController = new(_personsService, _countriesService, _logger);

        var actionResult = await personsController.Create(addRequest);

        actionResult.Should().BeOfType<RedirectToActionResult>();

        var redirectAction = (RedirectToActionResult)actionResult;

        redirectAction.ActionName.Should().Be("Index");
        redirectAction.ControllerName.Should().Be("Persons");
    }
    #endregion
}
