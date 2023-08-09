namespace CombatAnalysis.Identity.DTO;

public class AccessTokenDto
{
    public string Id { get; set; }

    public string UserId { get; set; }

    public string Token { get; set; }

    public DateTimeOffset Expires { get; set; }
}
