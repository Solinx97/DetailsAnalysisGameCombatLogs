using System.ComponentModel.DataAnnotations;

namespace CombatAnalysisIdentity.Models;

public class RestoreDataModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}
