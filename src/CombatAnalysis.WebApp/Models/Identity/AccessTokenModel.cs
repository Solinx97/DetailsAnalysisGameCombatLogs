namespace CombatAnalysis.WebApp.Models.Identity;

public class AccessTokenModel
{
    public string AccessToken { get; set; }

    public string TokenType { get; set; }

    public int ExpiresInMinutes { get; set; }

    public string RefreshToken { get; set; }
}
