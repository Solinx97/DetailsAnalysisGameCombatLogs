using System.ComponentModel.DataAnnotations;

namespace CombatAnalysisIdentity.Models;

public class RegistrationDataModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password and confirm password should be equal")]
    public string ConfirmPassword { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTimeOffset Birthday { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public int PhoneNumber { get; set; }

    [Required]
    public string Country { get; set; }

    public string City { get; set; }

    [Required]
    [DataType(DataType.PostalCode)]
    public int PostalCode { get; set; }
}
