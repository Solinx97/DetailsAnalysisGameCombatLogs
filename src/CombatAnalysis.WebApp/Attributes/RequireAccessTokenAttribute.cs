using CombatAnalysis.WebApp.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CombatAnalysis.WebApp.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequireAccessTokenAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var _)
            && !context.HttpContext.Request.Cookies.TryGetValue(AuthenticationCookie.RefreshToken.ToString(), out var _))
        {
            context.Result = new UnauthorizedResult();

            return;
        }

        if (!context.HttpContext.Request.Cookies.TryGetValue(AuthenticationCookie.AccessToken.ToString(), out var _)
            && context.HttpContext.Request.Cookies.TryGetValue(AuthenticationCookie.RefreshToken.ToString(), out var _))
        {
            return;
        }

        await next();
    }
}
