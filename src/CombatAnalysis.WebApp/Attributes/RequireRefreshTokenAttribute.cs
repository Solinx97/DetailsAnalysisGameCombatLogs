using CombatAnalysis.WebApp.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CombatAnalysis.WebApp.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequireRefreshTokenAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Cookies.TryGetValue(AuthenticationCookie.RefreshToken.ToString(), out var refreshToken))
        {
            context.Result = new UnauthorizedResult();

            return;
        }

        context.HttpContext.Items[AuthenticationCookie.RefreshToken.ToString()] = refreshToken;
        base.OnActionExecuting(context);
    }
}
