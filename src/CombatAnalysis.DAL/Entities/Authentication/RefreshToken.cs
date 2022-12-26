namespace CombatAnalysis.DAL.Entities.Authentication;

public class RefreshToken
{
    public string Id { get; set; }

    public string UserId { get; set; }

    public string Token { get; set; }

    public DateTimeOffset Expires { get; set; }
}
