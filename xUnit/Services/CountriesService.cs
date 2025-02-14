using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService(bool initialize = true)
    {
        _countries = new List<Country>();

        if (initialize)
        {
            _countries.AddRange(
                [
                    new Country { CountryID = Guid.Parse("CCBE8E62-6081-4072-B36D-FA7987D50000"), CountryName = "USA" },
                    new Country
                    {
                        CountryID = Guid.Parse("ABBCA825-C334-4416-9BF3-64356F60CCD6"),
                        CountryName = "Canada",
                    },
                    new Country { CountryID = Guid.Parse("C01D8E8D-F6E8-404A-9C74-FCF869BE2F16"), CountryName = "UK" },
                    new Country
                    {
                        CountryID = Guid.Parse("CCBE8E62-6081-4072-B36D-FA7987D50000"),
                        CountryName = "India",
                    },
                    new Country
                    {
                        CountryID = Guid.Parse("29A38F00-C0C4-4359-876C-078EC8617CF7"),
                        CountryName = "Australia",
                    },
                ]
            );
        }
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
