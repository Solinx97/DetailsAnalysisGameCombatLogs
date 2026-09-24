using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.WoWGameData;

[ServiceFilter(typeof(RequireBattleNetAuthorizationAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class WoWUserController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly IMapper _mapper;

    public WoWUserController(IOptions<BattleNet> cluster, IHttpClientHelper httpClient, IMapper mapper)
    {
        _mapper = mapper;
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.BattleNetAPI;
        _httpClient.BaseAddressApi = "";
    }

    [HttpGet("getMounts")]
    public async Task<IActionResult> GetMounts(string regionName)
    {
        var responseMessage = await _httpClient.GetAsync($"profile/user/wow/collections/mounts?namespace=profile-{regionName}");
        var reputations = await responseMessage.Content.ReadFromJsonAsync<CharacterMountsResponse>();
        if (reputations == null)
        {
            return BadRequest();
        }

        var map = _mapper.Map<CharacterMountDto[]>(reputations.Mounts);
        return Ok(map);
    }
}
