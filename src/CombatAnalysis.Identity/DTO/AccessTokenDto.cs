namespace CombatAnalysis.Identity.DTO;

public class AccessTokenDto
{
    public string AccessToken { get; set; }

    public string TokenType { get; set; }

    public DateTimeOffset Expires { get; set; }

    public string RefreshToken { get; set; }
}
