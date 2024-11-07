using System.ComponentModel.DataAnnotations;

namespace CombatAnalysisIdentity.Models;

public class VerifyEmailModel
{
    [Required]
    public string Token { get; set; }
}
