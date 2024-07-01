using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CombatAnalysis.WebApp.Enums;

namespace CombatAnalysis.WebApp.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequireAccessTokenAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Cookies.TryGetValue(AuthenticationTokenType.AccessToken.ToString(), out var accessToken) 
            || string.IsNullOrEmpty(accessToken))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}
