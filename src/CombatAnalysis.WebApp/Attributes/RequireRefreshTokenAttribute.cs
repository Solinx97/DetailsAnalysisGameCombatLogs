using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CombatAnalysis.WebApp.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
internal class RequireRefreshTokenAttribute : ActionFilterAttribute
{
    private readonly IHttpClientHelper _httpClientHelper;

    public RequireRefreshTokenAttribute(IHttpClientHelper httpClientHelper)
    {
        _httpClientHelper = httpClientHelper;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.RefreshToken), out var refreshToken))
        {
            context.Result = new UnauthorizedResult();

            return;
        }

        context.HttpContext.Items[nameof(AuthenticationCookie.RefreshToken)] = refreshToken;
        _httpClientHelper.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", refreshToken);

        base.OnActionExecuting(context);
    }
}
