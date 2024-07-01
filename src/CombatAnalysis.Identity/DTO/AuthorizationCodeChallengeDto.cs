namespace CombatAnalysis.Identity.DTO;

public class AuthorizationCodeChallengeDto
{
    public string Id { get; set; }

    public string CodeChallenge { get; set; }

    public string CodeChallengeMethod { get; set; }

    public string RedirectUrl { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset ExpiryTime { get; set; }

    public string ClientId { get; set; }

    public bool IsUsed { get; set; }
}
