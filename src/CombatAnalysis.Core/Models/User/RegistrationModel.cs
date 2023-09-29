namespace CombatAnalysis.Core.Models.User;

public class RegistrationModel
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string PhoneNumber { get; set; }

    public DateTimeOffset Birthday { get; set; }
}
