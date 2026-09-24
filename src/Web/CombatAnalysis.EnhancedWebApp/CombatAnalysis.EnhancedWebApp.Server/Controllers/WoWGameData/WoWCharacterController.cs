using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Reputation;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Reputation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.WoWGameData;

[ServiceFilter(typeof(RequireBattleNetAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class WoWCharacterController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly IMapper _mapper;

    public WoWCharacterController(IOptions<BattleNet> cluster, IHttpClientHelper httpClient, IMapper mapper)
    {
        _mapper = mapper;
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.BattleNetAPI;
        _httpClient.BaseAddressApi = "";
    }

    [HttpGet("getReputations/{username}")]
    public async Task<IActionResult> GetReputations(string username, string serverName, string regionName)
    {
        var responseMessage = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.ToLower()}/reputations?namespace=profile-{regionName}");
        var reputations = await responseMessage.Content.ReadFromJsonAsync<CharacterReputaionsResponse>();
        if (reputations == null)
        {
            return BadRequest();
        }

        var map = _mapper.Map<CharacterReputationDto[]>(reputations.Reputations);
        return Ok(map);
    }

    [HttpGet("getMounts/{username}")]
    public async Task<IActionResult> GetMounts(string username, string serverName, string regionName)
    {
        var responseMessage = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.ToLower()}/collections/mounts?namespace=profile-{regionName}");
        var reputations = await responseMessage.Content.ReadFromJsonAsync<CharacterMountsResponse>();
        if (reputations == null)
        {
            return BadRequest();
        }

        var map = _mapper.Map<CharacterMountDto[]>(reputations.Mounts);
        return Ok(map);
    }
}
