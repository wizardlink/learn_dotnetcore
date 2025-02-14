using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts;

/// <summary>
/// Represents business logic for manipulating <see cref="Entities.Person" />
/// </summary>
public interface IPersonService
{
    /// <summary>
    /// Adds a new person into the list of persons
    /// </summary>
    /// <param name="addRequest">Person to add</param>
    /// <returns>Returns the same person details, along with the newly generated PersonID</returns>
    public PersonResponse AddPerson(PersonAddRequest? addRequest);

    /// <returns>Returns a list of objects of <see cref="PersonResponse" /></returns>
    public List<PersonResponse> GetAllPersons();

    /// <summary>
    /// Returns the person object based on the given person ID
    /// </summary>
    /// <param name="personID">Person ID to search</param>
    /// <returns>Returns matchin person object</returns>
    public PersonResponse? GetPersonById(Guid? personID);

    /// <summary>
    /// Returns all <see cref="PersonResponse" /> objects that matches with the given search field and search string.
    /// </summary>
    /// <param name="searchBy">Field to search by</param>
    /// <param name="searchValue">What to search by</param>
    /// <returns>
    /// All matching <see cref="PersonResponse" /> based on the given search field and search string.
    /// </returns>
    public List<PersonResponse> GetFilteredPersons(string? searchBy, object? searchValue);

    /// <summary>
    /// Returns sorted list of <see cref="PersonResponse"/>
    /// </summary>
    /// <param name="allPersons">Reprents list of persons to sort</param>
    /// <param name="sortBy">Name of the property (key), based on which the persons should be sorted</param>
    /// <param name="sortOptions">ASC or DESC</param>
    /// <returns>The sorted listed based on the property</returns>
    public List<PersonResponse> GetSortedPersons(
        List<PersonResponse> allPersons,
        string sortBy,
        SortOrderOptions sortOptions
    );

    /// <summary>
    /// Updates the specified person details
    /// </summary>
    /// <param name="request">The data to be updated</param>
    /// <returns>Returns <see cref="PersonResponse" /></returns>
    public PersonResponse UpdatePerson(PersonUpdateRequest? request);

    /// <summary>
    /// Deletes a person based on the given <see cref="Entities.Person.PersonID"/>
    /// </summary>
    /// <param name="personID">The ID of the person to be deleted</param>
    /// <returns>Returns true when deleted, otherwise false</returns>
    public bool DeletePerson(Guid? personID);
}
