using System.ComponentModel.DataAnnotations;

namespace CombatAnalysisIdentity.Models;

public class PasswordResetModel
{
    [Required]
    public string Token { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password and confirm password should be equal")]
    public string ConfirmPassword { get; set; }
}
