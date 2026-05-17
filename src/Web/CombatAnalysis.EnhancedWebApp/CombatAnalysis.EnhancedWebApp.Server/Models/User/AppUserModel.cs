namespace CombatAnalysis.EnhancedWebApp.Server.Models.User;

public class AppUserModel
{
    public string Id { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; }

    public DateTimeOffset Birthday { get; set; }

    public string AboutMe { get; set; } = string.Empty;

    public int Gender { get; set; }

    public string IdentityUserId { get; set; } = string.Empty;
}
