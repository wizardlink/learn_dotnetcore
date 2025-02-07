using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

/// <summary>
/// Represents the DTO class that contains the person details to update
/// </summary>
public class PersonUpdateRequest
{
    [Required(ErrorMessage = "PersonID cannot be null")]
    public Guid PersonID { get; set; }

    [Required(ErrorMessage = "PersonName can't be blank")]
    public string? PersonName { get; set; }

    [Required(ErrorMessage = "Email can't be blank")]
    [EmailAddress(ErrorMessage = "Email value should be a valid email")]
    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public GenderOptions? Gender { get; set; }

    public Guid CountryID { get; set; }

    public string? Address { get; set; }

    public bool ReceiveNewsLetters { get; set; }

    /// <summary>
    /// Converts the current object of PersonAddRequest into a new object of Person type
    /// </summary>
    /// <returns>The converted PersonAddRequest</returns>
    public Person ToPerson()
    {
        return new Person()
        {
            PersonID = PersonID,
            PersonName = PersonName,
            Email = Email,
            Gender = Gender.ToString(),
            Address = Address,
            CountryID = CountryID,
            ReceiveNewsLetters = ReceiveNewsLetters,
        };
    }
}
