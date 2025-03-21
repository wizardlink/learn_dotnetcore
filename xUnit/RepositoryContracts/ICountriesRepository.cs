using Entities;

namespace RepositoryContracts;

public interface ICountriesRepository
{
    public Task<Country> AddCountry(Country country);

    public Task<IEnumerable<Country>> GetAllCountries();

    public Task<Country?> GetCountryByCountryID(Guid countryId);

    public Task<Country?> GetCountryByCountryName(string countryName);
}
