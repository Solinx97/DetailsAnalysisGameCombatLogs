using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Interfaces;

namespace CombatAnalysis.WebApp.Middlewares;

internal class AuthTokenMiddleware
{
    private readonly RequestDelegate _next;

    public AuthTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenService tokenService)
    {
        await CheckAccessTokenAsync(context, tokenService);
    }

    private async Task CheckAccessTokenAsync(HttpContext context, ITokenService tokenService)
    {
        if (!context.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.RefreshToken), out var refreshToken))
        {
            await _next(context);

            return;
        }

        if (!context.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.AccessToken), out var accessToken)
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

        context.Response.Cookies.Append(nameof(AuthenticationCookie.RefreshToken), token.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = token.Expires.AddDays(Authentication.RefreshTokenExpiresDays)
        });
        context.Response.Cookies.Append(nameof(AuthenticationCookie.AccessToken), token.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = token.Expires,
        });

        await _next(context);
    }
}
