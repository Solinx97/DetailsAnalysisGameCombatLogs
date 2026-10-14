using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;
using CombatAnalysis.EnhancedWebApp.Server.Models.Community;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.Community;

[Route("api/v1/[controller]")]
[ApiController]
public class CommunityController(ICommunityApiClient httpClient) : ControllerBase
{
    private readonly ICommunityApiClient _httpClient = httpClient;

    [HttpGet]
    public async Task<IActionResult> Get(int page, int pageSize, CancellationToken cancellationToken)
    {
        var communities = await _httpClient.GetAsync(page, pageSize, cancellationToken);
        return Ok(communities);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var community = await _httpClient.GetByIdAsync(id, cancellationToken);
        return Ok(community);
    }

    [HttpGet("getByUserId/{appUserId}")]
    public async Task<IActionResult> GetByUserId(Guid appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var communities = await _httpClient.GetByUserIdAsync(appUserId, page, pageSize, cancellationToken);
        return Ok(communities);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommunityModel community, CancellationToken cancellationToken) 
    {
        var createdCommunity = await _httpClient.CreateAsync(community, cancellationToken);
        return Ok(createdCommunity);
    }

    [HttpPatch("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommunityPatch community, CancellationToken cancellationToken)
    {
        await _httpClient.UpdateAsync(id, community, cancellationToken);
        return NoContent();
    }

    [HttpPatch("updateRules/{id:int:min(1)}")]
    public async Task<IActionResult> UpdateRules(int id, [FromBody] CommunityRulesPatch rules, CancellationToken cancellationToken)
    {
        await _httpClient.UpdatePolicyAsync(id, rules, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
