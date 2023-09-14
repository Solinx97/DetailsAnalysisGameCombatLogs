namespace CombatAnalysis.UserApi.Models;

public class RegisterModel
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string PhoneNumber { get; set; }

    public DateTimeOffset Birthday { get; set; }
}
