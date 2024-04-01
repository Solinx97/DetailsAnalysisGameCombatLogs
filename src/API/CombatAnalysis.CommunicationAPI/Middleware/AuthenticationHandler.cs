using CombatAnalysis.Identity.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CombatAnalysis.CommunicationAPI.Middleware;

public class BasicAuthenticationOptions : AuthenticationSchemeOptions
{
}

public class AuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
{
    private readonly IIdentityTokenService _tokenService;
    private readonly IJWTSecret _jwtSecret;

    public AuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock,
        IIdentityTokenService tokenService, IJWTSecret jWTSecret)
        : base(options, logger, encoder, clock)
    {
        _tokenService = tokenService;
        _jwtSecret = jWTSecret;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Request.Headers.Authorization.Count == 0)
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        string authorizationHeader = Request.Headers.Authorization;
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return AuthenticateResult.NoResult();
        }

        if (!authorizationHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        var token = authorizationHeader.Substring("bearer".Length).Trim();
        if (string.IsNullOrEmpty(token))
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        try
        {
            return await ValidateTokenAsync(token);
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail(ex.Message);
        }
    }

    private async Task<AuthenticateResult> ValidateTokenAsync(string token)
    {
        var secret = await _jwtSecret.GetSecretAsync();
        if (secret == null)
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        var claimsByRefreshToken = _tokenService.ValidateToken(token, secret.RefreshSecret, out var _);
        if (!claimsByRefreshToken.Any())
        {
            return AuthenticateResult.Fail("Unauthorized");
        }

        var identity = new ClaimsIdentity(claimsByRefreshToken, Scheme.Name);
        var principal = new System.Security.Principal.GenericPrincipal(identity, null);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
