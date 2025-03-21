using System.Linq.Expressions;
using Entities;

namespace RepositoryContracts;

public interface IPersonsRepository
{
    public Task<Person> AddPerson(Person person);

    public Task<IEnumerable<Person>> GetAllPersons();

    public Task<Person?> GetPersonByPersonID(Guid personId);

    public Task<IEnumerable<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

    public Task<bool> DeletePersonByPersonID(Guid personId);

    public Task<Person> UpdatePerson(Person person);
}
