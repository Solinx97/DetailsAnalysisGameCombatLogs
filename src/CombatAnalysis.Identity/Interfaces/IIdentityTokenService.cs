using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CombatAnalysis.Identity.Interfaces;

public interface IIdentityTokenService
{
    Task<Tuple<string, string>> GenerateTokensAsync(IResponseCookies cookies, string userId);

    IEnumerable<Claim> ValidateToken(string token, string secretKey, out SecurityToken validatedToken);
}
