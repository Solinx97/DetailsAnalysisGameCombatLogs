using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Interfaces;

namespace CombatAnalysis.WebApp.Middlewares;

public class TokenRefreshMiddleware
{
    private readonly RequestDelegate _next;

    public TokenRefreshMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenService tokenService)
    {
        if (!context.Request.Cookies.TryGetValue(AuthenticationTokenType.RefreshToken.ToString(), out var refreshToken))
        {
            await _next(context);

            return;
        }

        if (!context.Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken)
            && string.IsNullOrEmpty(refreshToken))
        {
            await _next(context);

            return;
        }

        if (!string.IsNullOrEmpty(accessToken) && !tokenService.IsAccessTokenCloseToExpiry(accessToken))
        {
            await _next(context);

            return;
        }

        var token = await tokenService.RefreshAccessTokenAsync(refreshToken);
        if (token == null)
        {
            await _next(context);

            return;
        }

        context.Response.Cookies.Append(AuthenticationTokenType.AccessToken.ToString(), token.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = token.Expires,
        });
        context.Response.Cookies.Append(AuthenticationTokenType.RefreshToken.ToString(), token.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = token.Expires.AddDays(Authentication.RefreshTokenExpiresDays)
        });

        await _next(context);
    }
}
