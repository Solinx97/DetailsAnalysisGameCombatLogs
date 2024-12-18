using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CombatAnalysis.WebApp.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequireAccessTokenAttribute : ActionFilterAttribute
{
    private readonly IHttpClientHelper _httpClientHelper;

    public RequireAccessTokenAttribute(IHttpClientHelper httpClientHelper)
    {
        _httpClientHelper = httpClientHelper;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Cookies.TryGetValue(AuthenticationCookie.RefreshToken.ToString(), out var _))
        {
            context.Result = new UnauthorizedResult();

            return;
        }

        if (!context.HttpContext.Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var accessToken))
        {
            context.Result = new UnauthorizedResult();

            return;
        }

        context.HttpContext.Items[AuthenticationCookie.AccessToken.ToString()] = accessToken;
        _httpClientHelper.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        base.OnActionExecuting(context);
    }
}
