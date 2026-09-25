using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Reputation;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.WoWGameData;

[Route("api/v1/[controller]")]
[ApiController]
public class WoWCharacterController(IWoWCharacterGameDataApiClient httpClient, IAchievementService achivmentService, IMapper mapper) : ControllerBase
{
    private readonly IWoWCharacterGameDataApiClient _httpClient = httpClient;
    private readonly IAchievementService _achivmentService = achivmentService;
    private readonly IMapper _mapper = mapper;

    [HttpGet("getReputations/{username}")]
    public async Task<IActionResult> GetReputations(string username, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var reputations = await _httpClient.GetReputationsAsync(serverName, username, regionName, cancellationToken);
        var map = _mapper.Map<CharacterReputationDto[]>(reputations.Reputations);
        return Ok(map);
    }

    [HttpGet("getMounts/{username}")]
    public async Task<IActionResult> GetMounts(string username, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var mounts = await _httpClient.GetMountsAsync(serverName, username, regionName, cancellationToken);
        var map = _mapper.Map<CharacterMountDto[]>(mounts.Mounts);
        return Ok(map);
    }

    [HttpGet("getProfileSummary/{username}")]
    public async Task<IActionResult> GetProfileSummary(string username, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var summary = await _httpClient.GetProfileSummaryAsync(serverName, username, regionName, cancellationToken);
        var map = _mapper.Map<CharacterDto>(summary);
        return Ok(map);
    }

    [HttpGet("getMythicKeystone/{username}")]
    public async Task<IActionResult> GetMythicKeystone(string username, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var mythicKeystone = await _httpClient.GetMythicKeystoneAsync(serverName, username, regionName, cancellationToken);
        var map = _mapper.Map<MythicKeystoneDto>(mythicKeystone);
        return Ok(map);
    }

    [HttpGet("getRaids/{username}")]
    public async Task<IActionResult> GetRaids(string username, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var raids = await _httpClient.GetRaidsAsync(serverName, username, regionName, cancellationToken);
        var map = _mapper.Map<CharacterDungeonDto>(raids);
        return Ok(map);
    }

    [HttpGet("getDungeons/{username}")]
    public async Task<IActionResult> GetDungeons(string username, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var dungeons = await _httpClient.GetDungeonsAsync(serverName, username, regionName, cancellationToken);
        var map = _mapper.Map<CharacterDungeonDto>(dungeons);
        return Ok(map);
    }

    [HttpGet("getAchievementCategory/{username}")]
    public async Task<IActionResult> GetAchievementCategory(string username, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var categories = await _achivmentService.GetCharacterAchievementCategoryAsync(serverName, username, regionName, cancellationToken);
        var map = _mapper.Map<AchievementCategoriesDto>(categories);
        return Ok(map);
    }

    [HttpGet("getAchievementsByCategory/{categoryId:int:min(1)}")]
    public async Task<IActionResult> GetAchievementsByCategory(int categoryId, string username, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var category = await _achivmentService.GetCharacterAchievementCategoryAsync(serverName, username, regionName, categoryId, cancellationToken);
        var map = _mapper.Map<AchievementSelectedCategoryDto>(category);
        return Ok(map);
    }
}
