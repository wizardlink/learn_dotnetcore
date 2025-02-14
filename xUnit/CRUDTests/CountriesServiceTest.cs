using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;

    public CountriesServiceTest()
    {
        _countriesService = new CountriesService(false);
    }

    #region AddCountry
    // When CountryAddRequest is null, it should throw ArgumentNullException
    [Fact]
    public void AddCountry_NullCountry()
    {
        CountryAddRequest? request = null;

        Assert.Throws<ArgumentNullException>(() =>
        {
            _countriesService.AddCountry(request);
        });
    }

    // When the CountryName is null it should throw ArgumentException
    [Fact]
    public void AddCountry_CountryNameIsNull()
    {
        CountryAddRequest? request = new CountryAddRequest { CountryName = null };

        Assert.Throws<ArgumentException>(() =>
        {
            _countriesService.AddCountry(request);
        });
    }

    // When the CountryName is duplicate, it should throw ArgumentException
    [Fact]
    public void AddCountry_DuplicateCountryName()
    {
        CountryAddRequest? request = new CountryAddRequest { CountryName = "USA" };
        CountryAddRequest? requestTwo = new CountryAddRequest { CountryName = "USA" };

        Assert.Throws<ArgumentException>(() =>
        {
            _countriesService.AddCountry(request);
            _countriesService.AddCountry(requestTwo);
        });
    }

    [Fact]
    public void AddCountry_ProperCountryDetails()
    {
        CountryAddRequest? request = new CountryAddRequest { CountryName = "Japan" };

        CountryResponse result = _countriesService.AddCountry(request);
        List<CountryResponse> countriesFromGetAllCountries = _countriesService.GetAllCountries();

        Assert.True(result.CountryID != Guid.Empty);
        Assert.Contains(result, countriesFromGetAllCountries);
    }
    #endregion

    #region GetAllCountries
    [Fact]
    // The list of countries should be empty by default (before adding any countries)
    public void GetAllCountries_EmptyList()
    {
        Assert.Empty(_countriesService.GetAllCountries());
    }

    [Fact]
    public void GetAllCountries_AddFewCountries()
    {
        List<CountryAddRequest> countryRequestList = new List<CountryAddRequest>()
        {
            new CountryAddRequest() { CountryName = "USA" },
            new CountryAddRequest() { CountryName = "UK" },
        };
        List<CountryResponse> countryResponseList = new List<CountryResponse>();

        foreach (CountryAddRequest countryRequest in countryRequestList)
        {
            countryResponseList.Add(_countriesService.AddCountry(countryRequest));
        }

        Assert.Equal(_countriesService.GetAllCountries(), countryResponseList);
    }
    #endregion
    #region GetCountryByCountryID
    [Fact]
    // If we supply null as CountryID, it should return null
    public void GetCountryByCountryID_NullCountryID()
    {
        Guid? countryId = null;

        CountryResponse? response = _countriesService.GetCountryByCountryID(countryId);

        Assert.Null(response);
    }

    [Fact]
    // If we supply a valid country id, it should return the matching country details
    public void GetCountryByCountryID_ValidCountryID()
    {
        CountryAddRequest? addRequest = new CountryAddRequest() { CountryName = "China" };

        var response = _countriesService.AddCountry(addRequest);

        var countryById = _countriesService.GetCountryByCountryID(response.CountryID);

        Assert.Equal(response, countryById);
    }
    #endregion
}
