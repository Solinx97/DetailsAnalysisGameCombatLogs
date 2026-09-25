using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Reputation;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;
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
        var responseMessage = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.ToLower()}/reputations?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}");
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
        var responseMessage = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.ToLower()}/collections/mounts?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}");
        var mounts = await responseMessage.Content.ReadFromJsonAsync<CharacterMountsResponse>();
        if (mounts == null)
        {
            return BadRequest();
        }

        var map = _mapper.Map<CharacterMountDto[]>(mounts.Mounts);
        return Ok(map);
    }

    [HttpGet("getProfileSummary/{username}")]
    public async Task<IActionResult> GetProfileSummary(string username, string serverName, string regionName)
    {
        var responseMessage = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.ToLower()}?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}");
        var summary = await responseMessage.Content.ReadFromJsonAsync<CharacterModel>();
        if (summary == null)
        {
            return BadRequest();
        }

        var map = _mapper.Map<CharacterDto>(summary);
        return Ok(map);
    }

    [HttpGet("getMythicKeystone/{username}")]
    public async Task<IActionResult> GetMythicKeystone(string username, string serverName, string regionName)
    {
        var responseMessage = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.ToLower()}/mythic-keystone-profile?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}");
        var mythicKeystone = await responseMessage.Content.ReadFromJsonAsync<MythicKeystoneModel>();
        if (mythicKeystone == null)
        {
            return BadRequest();
        }

        var map = _mapper.Map<MythicKeystoneDto>(mythicKeystone);
        return Ok(map);
    }

    [HttpGet("getRaids/{username}")]
    public async Task<IActionResult> GetRaids(string username, string serverName, string regionName)
    {
        var responseMessage = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.ToLower()}/encounters/raids?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}");
        var raids = await responseMessage.Content.ReadFromJsonAsync<CharacterDungeonModel>();
        if (raids == null)
        {
            return BadRequest();
        }

        var map = _mapper.Map<CharacterDungeonDto>(raids);
        return Ok(map);
    }

    [HttpGet("getDungeons/{username}")]
    public async Task<IActionResult> GetDungeons(string username, string serverName, string regionName)
    {
        var responseMessage = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.ToLower()}/encounters/dungeons?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}");
        var dungeons = await responseMessage.Content.ReadFromJsonAsync<CharacterDungeonModel>();
        if (dungeons == null)
        {
            return BadRequest();
        }

        var map = _mapper.Map<CharacterDungeonDto>(dungeons);
        return Ok(map);
    }

    [HttpGet("getAchievements/{username}")]
    public async Task<IActionResult> GetAchievements(string username, string serverName, string regionName)
    {
        var responseMessage = await _httpClient.GetAsync($"profile/wow/character/{serverName}/{username.ToLower()}/achievements?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}");
        var achievements = await responseMessage.Content.ReadFromJsonAsync<CharacterAchievementsModel>();
        if (achievements == null)
        {
            return BadRequest();
        }

        var map = _mapper.Map<CharacterAchievementsDto>(achievements);
        return Ok(map);
    }
}
