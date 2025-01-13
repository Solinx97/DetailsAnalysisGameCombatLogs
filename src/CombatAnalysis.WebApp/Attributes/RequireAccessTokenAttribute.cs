using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CombatAnalysis.WebApp.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
internal class RequireAccessTokenAttribute : ActionFilterAttribute
{
    private readonly IHttpClientHelper _httpClientHelper;

    public RequireAccessTokenAttribute(IHttpClientHelper httpClientHelper)
    {
        _httpClientHelper = httpClientHelper;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.RefreshToken), out var _))
        {
            context.Result = new UnauthorizedResult();

            return;
        }

        if (!context.HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.AccessToken), out var accessToken))
        {
            context.Result = new UnauthorizedResult();

            return;
        }

        context.HttpContext.Items[nameof(AuthenticationCookie.AccessToken)] = accessToken;
        _httpClientHelper.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        base.OnActionExecuting(context);
    }
}
