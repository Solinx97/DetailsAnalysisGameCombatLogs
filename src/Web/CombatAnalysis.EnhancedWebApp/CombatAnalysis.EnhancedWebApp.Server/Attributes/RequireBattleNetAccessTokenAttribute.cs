using CombatAnalysis.EnhancedWebApp.Server.Enums;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CombatAnalysis.EnhancedWebApp.Server.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
internal class RequireBattleNetAccessTokenAttribute(IHttpClientHelper httpClientHelper) : ActionFilterAttribute
{
    private readonly IHttpClientHelper _httpClientHelper = httpClientHelper;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.BattleNetAccessToken), out var accessToken))
        {
            context.Result = new UnauthorizedResult();

            return;
        }

        _httpClientHelper.AddAuthorizationHeader("Bearer", accessToken);

        base.OnActionExecuting(context);
    }
}
