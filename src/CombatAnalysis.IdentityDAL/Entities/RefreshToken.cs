namespace CombatAnalysis.IdentityDAL.Entities;

public class RefreshToken
{
    public string Id { get; set; }

    public string Token { get; set; }

    public DateTimeOffset ExpiryTime { get; set; }

    public string ClientId { get; set; }

    public string UserId { get; set; }
}
