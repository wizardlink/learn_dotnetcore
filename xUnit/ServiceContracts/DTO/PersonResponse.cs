using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

/// <summary>
/// Represents DTO class that is used as a return type of most methods of Persons Service
/// </summary>
public class PersonResponse
{
    public Guid PersonID { get; set; }

    public string? PersonName { get; set; }

    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public Guid CountryID { get; set; }

    public string? Country { get; set; }

    public string? Address { get; set; }

    public bool ReceiveNewsLetters { get; set; }

    public double? Age { get; set; }

    /// <summary>
    /// Compares the current object data with the parameter object
    /// </summary>
    /// <param name="obj">The <see cref="PersonResponse" /> Object to compare</param>
    /// <returns>Whether all fields match each other</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (obj.GetType() != typeof(PersonResponse))
            return false;

        var response = (PersonResponse)obj;

        foreach (var member in typeof(PersonResponse).GetFields())
        {
            if (
                this.GetType().GetField(member.Name)?.GetValue(this)
                != response.GetType().GetField(member.Name)?.GetValue(response)
            )
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"""
            ID - {PersonID}
            Name - {PersonName}
            Date of Birth - {DateOfBirth?.ToString("dd MMM yyyy")}
            Gender - {Gender}
            Country ID - {CountryID}
            Country - {Country}
            Address - {Address}
            Receive News Letters - {ReceiveNewsLetters}
            """;
    }

    public PersonUpdateRequest ToPersonUpdateRequest()
    {
        return new PersonUpdateRequest()
        {
            PersonID = PersonID,
            Email = Email,
            DateOfBirth = DateOfBirth,
            Gender = Gender == null ? null : Enum.Parse<GenderOptions>(Gender),
            Address = Address,
            CountryID = CountryID,
            ReceiveNewsLetters = ReceiveNewsLetters,
        };
    }
}

public static class PersonExtensions
{
    /// <summary>
    /// An extension method to convert an object of <see cref="Person" /> class into <see cref="PersonResponse" /> class
    /// </summary>
    /// <param name="person">Returns the converted <see cref="PersonResponse" /> object</param>
    /// <returns>Returns the converted <see cref="PersonResponse" /> object</returns>
    public static PersonResponse ToPersonResponse(this Person person)
    {
        return new PersonResponse()
        {
            PersonID = person.PersonID,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            ReceiveNewsLetters = person.ReceiveNewsLetters,
            Address = person.Address,
            CountryID = person.CountryID,
            Gender = person.Gender,
            Age =
                person.DateOfBirth != null
                    ? Math.Round(((TimeSpan)(DateTime.Now - person.DateOfBirth)).TotalDays / 365.25)
                    : null,
        };
    }
}
