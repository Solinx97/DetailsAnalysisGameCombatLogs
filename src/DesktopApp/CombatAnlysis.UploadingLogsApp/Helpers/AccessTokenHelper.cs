using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace CombatAnalysis.UploadingLogsApp.Helpers;

internal static class AccessTokenHelper
{
    public static string? GetUserIdFromToken(string? token)
    {
        ArgumentException.ThrowIfNullOrEmpty(token, nameof(token));

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        var userIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "sub");

        return userIdClaim?.Value;
    }
}
