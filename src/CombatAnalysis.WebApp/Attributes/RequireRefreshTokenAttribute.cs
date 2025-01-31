using CombatAnalysis.WebApp.Enums;
using CombatAnalysis.WebApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CombatAnalysis.WebApp.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
internal class RequireRefreshTokenAttribute(IHttpClientHelper httpClientHelper) : ActionFilterAttribute
{
    private readonly IHttpClientHelper _httpClientHelper = httpClientHelper;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.RefreshToken), out var refreshToken))
        {
            context.Result = new UnauthorizedResult();

            return;
        }

        _httpClientHelper.AddAuthorizationHeader("Bearer", refreshToken);

        base.OnActionExecuting(context);
    }
}
