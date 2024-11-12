using CombatAnalysis.WebApp.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CombatAnalysis.WebApp.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequireAccessTokenAttribute : ActionFilterAttribute
{
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
        base.OnActionExecuting(context);
    }
}
