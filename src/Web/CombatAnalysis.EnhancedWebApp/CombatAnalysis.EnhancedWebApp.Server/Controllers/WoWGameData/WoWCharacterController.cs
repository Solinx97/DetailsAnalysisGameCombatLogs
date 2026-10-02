using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account.Collections;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Equipments;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Professions;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Reputation;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.WoWGameData;

[Route("api/v1/[controller]")]
[ApiController]
public class WoWCharacterController(IWoWCharacterGameDataApiClient httpClient, IAchievementService achivmentService, IWoWCharacterService characterService, IMapper mapper) : ControllerBase
{
    private readonly IWoWCharacterGameDataApiClient _httpClient = httpClient;
    private readonly IAchievementService _achivmentService = achivmentService;
    private readonly IWoWCharacterService _characterService = characterService;
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
        var map = _mapper.Map<WoWAccountCollectionItemDto[]>(mounts.Mounts);
        return Ok(map);
    }

    [HttpGet("getProfileSummary/{username}")]
    public async Task<IActionResult> GetProfileSummary(string username, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var summary = await _httpClient.GetProfileSummaryAsync(serverName, username, regionName, cancellationToken);
        var map = _mapper.Map<CharacterSummaryDto>(summary);
        return Ok(map);
    }

    [HttpGet("getEquipments/{username}")]
    public async Task<IActionResult> GetEquipments(string username, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var equipments = await _httpClient.GetEquipmentsAsync(serverName, username, regionName, cancellationToken);
        var map = _mapper.Map<CharacterEquipmentsResponseDto>(equipments);
        return Ok(map);
    }

    [HttpGet("getStats/{username}")]
    public async Task<IActionResult> GetStats(string username, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var equipments = await _httpClient.GetStatsAsync(serverName, username, regionName, cancellationToken);
        var map = _mapper.Map<CharacterStatsDto>(equipments);
        return Ok(map);
    }

    [HttpGet("getMythicKeystone/{username}")]
    public async Task<IActionResult> GetMythicKeystone(string username, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var mythicKeystone = await _httpClient.GetMythicKeystoneAsync(serverName, username, regionName, cancellationToken);
        var map = _mapper.Map<MythicKeystoneDto>(mythicKeystone);
        return Ok(map);
    }

    [HttpGet("getMythicKeystoneSeason/{username}")]
    public async Task<IActionResult> GetMythicKeystoneSeason(string username, int seasonId, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var mythicKeystoneSeason = await _httpClient.GetMythicKeystoneSeasonAsync(serverName, seasonId, username, regionName, cancellationToken);
        var map = _mapper.Map<MythicKeystoneSeasonDto>(mythicKeystoneSeason);
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

    [HttpGet("getAchievementsCategory/{username}")]
    public async Task<IActionResult> GetAchievementsCategory(string username, string serverName, string regionName, CancellationToken cancellationToken)
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

    [HttpGet("getAchievementsStatisticsCategory/{username}")]
    public async Task<IActionResult> GetAchievementsStatisticsCategory(string username, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var statistics = await _httpClient.GetAchievementStatisticsAsync(serverName, username, regionName, cancellationToken);
        var map = _mapper.Map<CharacterAchievementStatisticsCategoryDto[]>(statistics.Categories);
        return Ok(map);
    }

    [HttpGet("getProfessions/{username}")]
    public async Task<IActionResult> GetProfessions(string username, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var professions = await _httpClient.GetProfessionsAsync(serverName, username, regionName, cancellationToken);
        var map = _mapper.Map<CharacterProfessionsResponseDto>(professions);
        return Ok(map);
    }

    [HttpGet("getDecors/{username}")]
    public async Task<IActionResult> GetDecors(string username, string serverName, string regionName, CancellationToken cancellationToken)
    {
        var decors = await _characterService.GetDecorsAsync(regionName, serverName, username, cancellationToken);
        return Ok(decors);
    }
}
