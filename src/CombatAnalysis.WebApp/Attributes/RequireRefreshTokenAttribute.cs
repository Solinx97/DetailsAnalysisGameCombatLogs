using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CombatAnalysis.WebApp.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequireRefreshTokenAttribute : ActionFilterAttribute
{
    private readonly IHttpClientHelper _httpClientHelper;

    public RequireRefreshTokenAttribute(IHttpClientHelper httpClientHelper)
    {
        _httpClientHelper = httpClientHelper;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Cookies.TryGetValue(AuthenticationCookie.RefreshToken.ToString(), out var refreshToken))
        {
            context.Result = new UnauthorizedResult();

            return;
        }

        context.HttpContext.Items[AuthenticationCookie.RefreshToken.ToString()] = refreshToken;
        _httpClientHelper.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", refreshToken);

        base.OnActionExecuting(context);
    }
}
