using System.Linq.Expressions;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;

namespace Repositories;

public class PersonsRepository : IPersonsRepository
{
    private readonly ApplicationDbContext _database;
    private readonly ILogger<PersonsRepository> _logger;

    public PersonsRepository(ApplicationDbContext database, ILogger<PersonsRepository> logger)
    {
        _database = database;
        _logger = logger;
    }

    public async Task<Person> AddPerson(Person person)
    {
        _database.Persons.Add(person);
        await _database.SaveChangesAsync();

        return person;
    }

    public async Task<bool> DeletePersonByPersonID(Guid personId)
    {
        _database.Persons.RemoveRange(_database.Persons.Where(table => table.PersonID == personId));
        int numberOfChanges = await _database.SaveChangesAsync();

        return numberOfChanges > 0;
    }

    public async Task<IEnumerable<Person>> GetAllPersons()
    {
        return await _database.Persons.Include("Country").ToListAsync();
    }

    public async Task<IEnumerable<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
    {
        _logger.LogInformation("GetFilteredPersons of PersonsRepository");
        return await _database.Persons.Include("Country").Where(predicate).ToListAsync();
    }

    public async Task<Person?> GetPersonByPersonID(Guid personId)
    {
        return await _database.Persons.Include("Country").FirstOrDefaultAsync(table => table.PersonID == personId);
    }

    public async Task<Person> UpdatePerson(Person person)
    {
        Person? dbSearch = await _database.Persons.FirstOrDefaultAsync(table => table.PersonID == person.PersonID);

        if (dbSearch != null)
        {
            _database.Update(person);
            await _database.SaveChangesAsync();
        }

        return person;
    }
}
