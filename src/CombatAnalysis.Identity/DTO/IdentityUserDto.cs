namespace CombatAnalysis.Identity.DTO;

public class IdentityUserDto
{
    public string Id { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string Salt { get; set; }

    public bool EmailVerified { get; set; }
}
