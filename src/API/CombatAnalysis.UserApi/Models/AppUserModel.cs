namespace CombatAnalysis.UserApi.Models;

public class AppUserModel
{
    public string Id { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public DateTimeOffset Birthday { get; set; }

    public string Password { get; set; }
}
