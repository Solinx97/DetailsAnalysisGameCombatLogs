using CombatAnalysis.Identity.Interfaces;
using CombatAnalysisIdentity.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysisIdentity.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IIdentityUserService _identityUserService;

    public IdentityController(IIdentityUserService identityUserService)
    {
        _identityUserService = identityUserService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmail(string id)
    {
        var identityUser = await _identityUserService.GetByIdAsync(id);
        if (identityUser == null)
        {
            return BadRequest();
        }

        var identityUserPrivacy = new IdentityUserPrivacyModel
        {
            Email = identityUser.Email,
            EmailVerified = identityUser.EmailVerified,
        };

        return Ok(identityUserPrivacy);
    }
}
