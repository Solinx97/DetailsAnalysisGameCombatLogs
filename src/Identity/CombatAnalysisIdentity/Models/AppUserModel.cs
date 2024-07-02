namespace CombatAnalysisIdentity.Models;

public class AppUserModel
{
    public string Id { get; set; } = string.Empty;

    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int PhoneNumber { get; set; }

    public DateTimeOffset Birthday { get; set; }

    public string AboutMe { get; set; }

    public int Gender { get; set; }

    public string IdentityUserId { get; set; }
}
