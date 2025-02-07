using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService()
    {
        _countries = new List<Country>();
    }

    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
        // countryAddRequest can't be null
        if (countryAddRequest == null)
        {
            throw new ArgumentNullException(nameof(countryAddRequest));
        }

        // CountryName can't be null
        if (countryAddRequest.CountryName == null)
        {
            throw new ArgumentException(nameof(countryAddRequest.CountryName));
        }

        if (_countries.Exists((country) => country.CountryName == countryAddRequest.CountryName))
        {
            throw new ArgumentException("Given country already exists.");
        }

        // Convert object from CountryAddRequest to Country type
        Country country = countryAddRequest.ToCountry();

        // Generate the country's guid
        country.CountryID = Guid.NewGuid();

        // Add country object into _countries
        _countries.Add(country);

        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries()
    {
        var countries = _countries.Select(country => country.ToCountryResponse()).ToList();

        return countries;
    }

    public CountryResponse? GetCountryByCountryID(Guid? countryId)
    {
        if (countryId == null)
            return null;

        Country? country = _countries.FirstOrDefault(country => country.CountryID == countryId);

        if (country == null)
            return null;

        return country.ToCountryResponse();
    }
}
