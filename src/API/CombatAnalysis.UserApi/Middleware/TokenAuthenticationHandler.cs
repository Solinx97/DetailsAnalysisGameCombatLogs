using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CombatAnalysis.UserApi.Middleware;

public class TokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TokenAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        if (string.IsNullOrEmpty(token))
        {
            return AuthenticateResult.Fail("Token is missing.");
        }

        bool isValidToken = ValidateToken(token);
        if (!isValidToken)
        {
            return AuthenticateResult.Fail("Invalid token.");
        }

        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "UserId") }; // Customize claims as needed
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    private bool ValidateToken(string token)
    {
        // Implement your token validation logic here
        // Placeholder for demonstration purposes
        return true; // Assume token is valid
    }
}
