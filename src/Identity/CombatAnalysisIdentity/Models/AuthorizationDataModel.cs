using System.ComponentModel.DataAnnotations;

namespace CombatAnalysisIdentity.Models;

public class AuthorizationDataModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
