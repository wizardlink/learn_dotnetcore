using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories;

public class CountriesRepository : ICountriesRepository
{
    private readonly ApplicationDbContext _database;

    public CountriesRepository(ApplicationDbContext database)
    {
        _database = database;
    }

    public async Task<Country> AddCountry(Country country)
    {
        _database.Countries.Add(country);
        await _database.SaveChangesAsync();

        return country;
    }

    public async Task<IEnumerable<Country>> GetAllCountries()
    {
        return await _database.Countries.ToListAsync();
    }

    public async Task<Country?> GetCountryByCountryID(Guid countryId)
    {
        return await _database.Countries.FirstOrDefaultAsync(table => table.CountryID == countryId);
    }

    public async Task<Country?> GetCountryByCountryName(string countryName)
    {
        return await _database.Countries.FirstOrDefaultAsync(table => table.CountryName == countryName);
    }
}
