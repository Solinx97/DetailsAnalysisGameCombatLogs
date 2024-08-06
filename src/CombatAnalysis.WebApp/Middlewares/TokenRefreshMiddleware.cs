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
        //var endpoint = context.GetEndpoint();
        //if (endpoint != null)
        //{
        //    var authorizeAttribute = endpoint.Metadata.GetMetadata<RequireAccessTokenAttribute>();
        //    if (authorizeAttribute != null)
        //    {
        //        await _next(context);

        //        return;
        //    }
        //}

        await CheckAccessTokenAsync(context, tokenService);
    }

    private async Task CheckAccessTokenAsync(HttpContext context, ITokenService tokenService)
    {
        if (!context.Request.Cookies.TryGetValue(AuthenticationCookie.RefreshToken.ToString(), out var refreshToken))
        {
            await _next(context);

            return;
        }

        if (!context.Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken)
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

        context.Response.Cookies.Append(AuthenticationCookie.AccessToken.ToString(), token.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = token.Expires,
        });
        context.Response.Cookies.Append(AuthenticationCookie.RefreshToken.ToString(), token.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = token.Expires.AddDays(Authentication.RefreshTokenExpiresDays)
        });

        await _next(context);
    }
}
