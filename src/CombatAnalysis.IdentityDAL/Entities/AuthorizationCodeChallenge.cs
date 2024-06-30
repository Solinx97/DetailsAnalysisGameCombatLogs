namespace CombatAnalysis.IdentityDAL.Entities;

public class AuthorizationCodeChallenge
{
    public string Id { get; set; }

    public string CodeChallenge { get; set; }

    public string CodeChallengeMethod { get; set; }

    public string RequestId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string ClientId { get; set; }

    public string SessionId { get; set; }
}
