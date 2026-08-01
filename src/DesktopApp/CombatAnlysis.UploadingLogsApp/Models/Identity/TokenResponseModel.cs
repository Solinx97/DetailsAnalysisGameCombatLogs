using System.Text.Json.Serialization;

namespace CombatAnalysis.UploadingLogsApp.Models.Identity;

public class TokenResponseModel
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("scope")]
    public string Scope { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresInHours { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    public int RefreshTokenExpiresInHours { get; set; } = 720;
}
