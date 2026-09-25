using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.Attributes;
using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.WoWGameData;

[ServiceFilter(typeof(RequireBattleNetAccessTokenAttribute))]
[Route("api/v1/[controller]")]
[ApiController]
public class WoWDataController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;
    private readonly IAchivmentService _achivmentService;
    private readonly IMapper _mapper;

    public WoWDataController(IOptions<BattleNet> cluster, IHttpClientHelper httpClient, IAchivmentService achivmentService, IMapper mapper)
    {
        _mapper = mapper;
        _achivmentService = achivmentService;

        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.BattleNetAPI;
        _httpClient.BaseAddressApi = "";
    }

    [HttpGet("getRealms/{regionName}")]
    public async Task<IActionResult> GetRealms(string regionName)
    {
        var responseMessage = await _httpClient.GetAsync($"data/wow/realm/index?namespace=dynamic-{regionName}&locale={WoWDataLocale.Locale}");
        responseMessage.EnsureSuccessStatusCode();

        var realms = await responseMessage.Content.ReadFromJsonAsync<RealmsResponse>();
        if (realms == null)
        {
            return BadRequest();
        }

        var map = _mapper.Map<RealmDto[]>(realms.Realms);
        return Ok(map);
    }

    [HttpGet("getAchievementCategory/{username}")]
    public async Task<IActionResult> GetAchievementCategory(string username, string serverName, string regionName)
    {
        try
        {
            var categories = await _achivmentService.GetCharacterAchievementCategoryAsync(serverName, username, regionName);
            if (categories == null)
            {
                return BadRequest();
            }

            var map = _mapper.Map<AchievementCategoriesDto>(categories);
            return Ok(map);
        }
        catch (ArgumentNullException)
        {
            return BadRequest();
        }
    }

    [HttpGet("getAchievementsByCategory/{categoryId:int:min(1)}")]
    public async Task<IActionResult> GetAchievementsByCategory(int categoryId, string username, string serverName, string regionName)
    {
        try
        {
            var categories = await _achivmentService.GetCharacterAchievementCategoryAsync(serverName, username, regionName, categoryId);
            if (categories == null)
            {
                return BadRequest();
            }

            var map = _mapper.Map<AchievementSelectedCategoryDto>(categories);
            return Ok(map);
        }
        catch (ArgumentNullException)
        {
            return BadRequest();
        }
    }

    [HttpGet("getAchievement/{achievementId:int:min(1)}")]
    public async Task<IActionResult> GetAchievement(int achievementId, string regionName)
    {
        var responseMessage = await _httpClient.GetAsync($"data/wow/achievement/{achievementId}?namespace=static-{regionName}&locale={WoWDataLocale.Locale}");
        var achievement = await responseMessage.Content.ReadFromJsonAsync<SelectedAchievementModel>();
        if (achievement == null)
        {
            return BadRequest();
        }

        var map = _mapper.Map<SelectedAchievementDto>(achievement);
        return Ok(map);
    }
}
