namespace CombatAnalysis.UserDAL.Entities;

public class AppUser
{
    public string Id { get; set; }

    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int PhoneNumber { get; set; }

    public DateTimeOffset Birthday { get; set; }

    public string AboutMe { get; set; }

    public int Gender { get; set; }

    public string IdentityUserId { get; set; }
}
