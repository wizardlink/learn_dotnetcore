using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly PersonsDbContext _database;

    public CountriesService(PersonsDbContext personsDbContext)
    {
        _database = personsDbContext;
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

        if (await _database.Countries.AnyAsync((country) => country.CountryName == countryAddRequest.CountryName))
        {
            throw new ArgumentException("Given country already exists.");
        }

        // Convert object from CountryAddRequest to Country type
        Country country = countryAddRequest.ToCountry();

        // Generate the country's guid
        country.CountryID = Guid.NewGuid();

        // Add country object into _countries
        _database.Countries.Add(country);
        await _database.SaveChangesAsync();

        return country.ToCountryResponse();
    }

    public async Task<List<CountryResponse>> GetAllCountries()
    {
        var countries = await _database.Countries.Select(country => country.ToCountryResponse()).ToListAsync();

        return countries;
    }

    public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryId)
    {
        if (countryId == null)
            return null;

        Country? country = await _database.Countries.FirstOrDefaultAsync(country => country.CountryID == countryId);

        if (country == null)
            return null;

        return country.ToCountryResponse();
    }
}
