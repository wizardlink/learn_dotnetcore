using System.ComponentModel.DataAnnotations;

namespace Entities;

/// <summary>
/// Person domain model class
/// </summary>
public class Person
{
    [Key]
    public Guid PersonID { get; set; }

    public string? PersonName { get; set; }

    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [StringLength(10)]
    public string? Gender { get; set; }

    public Guid CountryID { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    public bool ReceiveNewsLetters { get; set; }

    public string? TIN { get; set; }

    public virtual Country? Country { get; set; }
}
