using Entities;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly ICountriesRepository _repository;

    public CountriesService(ICountriesRepository countriesRepository)
    {
        _repository = countriesRepository;
    }

    public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
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

        if (await _repository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
        {
            throw new ArgumentException("Given country already exists.");
        }

        // Convert object from CountryAddRequest to Country type
        Country country = countryAddRequest.ToCountry();

        // Generate the country's guid
        country.CountryID = Guid.NewGuid();

        // Add country object into _countries
        await _repository.AddCountry(country);

        return country.ToCountryResponse();
    }

    public async Task<List<CountryResponse>> GetAllCountries()
    {
        var countries = (await _repository.GetAllCountries()).Select(country => country.ToCountryResponse()).ToList();

        return countries;
    }

    public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryId)
    {
        if (!countryId.HasValue)
            return null;

        Country? country = await _repository.GetCountryByCountryID(countryId.Value);

        if (country == null)
            return null;

        return country.ToCountryResponse();
    }
}
