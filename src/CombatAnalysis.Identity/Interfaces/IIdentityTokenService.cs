using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CombatAnalysis.Identity.Interfaces;

public interface IIdentityTokenService
{
    Task<string> GenerateTokensAsync(string userId);

    IEnumerable<Claim> ValidateToken(string token, string secretKey, out SecurityToken validatedToken);
}
