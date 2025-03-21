using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests;

public class CountriesServiceTest
{
    private readonly CountriesService _countriesService;

    public CountriesServiceTest()
    {
        List<Country> initialCountryData = [];

        DbContextMock<ApplicationDbContext> dbContextMock = new(
            new DbContextOptionsBuilder<ApplicationDbContext>().Options
        );

        var dbContext = dbContextMock.Object;

        dbContextMock.CreateDbSetMock(db => db.Countries, initialCountryData);

        _countriesService = new CountriesService(null);
    }

    #region AddCountry
    // When CountryAddRequest is null, it should throw ArgumentNullException
    [Fact]
    public async Task AddCountry_NullCountry()
    {
        CountryAddRequest? request = null;

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
        {
            return _countriesService.AddCountry(request);
        });
    }

    // When the CountryName is null it should throw ArgumentException
    [Fact]
    public async Task AddCountry_CountryNameIsNull()
    {
        CountryAddRequest? request = new CountryAddRequest { CountryName = null };

        await Assert.ThrowsAsync<ArgumentException>(() =>
        {
            return _countriesService.AddCountry(request);
        });
    }

    // When the CountryName is duplicate, it should throw ArgumentException
    [Fact]
    public async Task AddCountry_DuplicateCountryName()
    {
        CountryAddRequest? request = new CountryAddRequest { CountryName = "USA" };
        CountryAddRequest? requestTwo = new CountryAddRequest { CountryName = "USA" };

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _countriesService.AddCountry(request);
            await _countriesService.AddCountry(requestTwo);
        });
    }

    [Fact]
    public async Task AddCountry_ProperCountryDetails()
    {
        CountryAddRequest? request = new CountryAddRequest { CountryName = "Japan" };

        CountryResponse result = await _countriesService.AddCountry(request);
        List<CountryResponse> countriesFromGetAllCountries = await _countriesService.GetAllCountries();

        Assert.True(result.CountryID != Guid.Empty);
        Assert.Contains(result, countriesFromGetAllCountries);
    }
    #endregion

    #region GetAllCountries
    [Fact]
    // The list of countries should be empty by default (before adding any countries)
    public async Task GetAllCountries_EmptyList()
    {
        Assert.Empty(await _countriesService.GetAllCountries());
    }

    [Fact]
    public async Task GetAllCountries_AddFewCountries()
    {
        List<CountryAddRequest> countryRequestList = new List<CountryAddRequest>()
        {
            new CountryAddRequest() { CountryName = "USA" },
            new CountryAddRequest() { CountryName = "UK" },
        };
        List<CountryResponse> countryResponseList = new List<CountryResponse>();

        foreach (CountryAddRequest countryRequest in countryRequestList)
        {
            countryResponseList.Add(await _countriesService.AddCountry(countryRequest));
        }

        Assert.Equal(await _countriesService.GetAllCountries(), countryResponseList);
    }
    #endregion
    #region GetCountryByCountryID
    [Fact]
    // If we supply null as CountryID, it should return null
    public async Task GetCountryByCountryID_NullCountryID()
    {
        Guid? countryId = null;

        CountryResponse? response = await _countriesService.GetCountryByCountryID(countryId);

        Assert.Null(response);
    }

    [Fact]
    // If we supply a valid country id, it should return the matching country details
    public async Task GetCountryByCountryID_ValidCountryID()
    {
        CountryAddRequest? addRequest = new CountryAddRequest() { CountryName = "China" };

        var response = await _countriesService.AddCountry(addRequest);

        var countryById = await _countriesService.GetCountryByCountryID(response.CountryID);

        Assert.Equal(response, countryById);
    }
    #endregion
}
