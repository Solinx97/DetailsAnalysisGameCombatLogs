using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.WoWGameData;

[Route("api/v1/[controller]")]
[ApiController]
public class WoWDataController(IWoWGameDataApiClient httpClient, IMapper mapper) : ControllerBase
{
    private readonly IWoWGameDataApiClient _httpClient = httpClient;
    private readonly IMapper _mapper = mapper;

    [HttpGet("getRealms/{regionName}")]
    public async Task<IActionResult> GetRealms(string regionName, CancellationToken cancellationToken)
    {
        var realms = await _httpClient.GetRealmsAsync(regionName, cancellationToken);
        var map = _mapper.Map<RealmDto[]>(realms.Realms);
        return Ok(map);
    }

    [HttpGet("getAchievement/{achievementId:int:min(1)}")]
    public async Task<IActionResult> GetAchievement(int achievementId, string regionName, CancellationToken cancellationToken)
    {
        var achievement = await _httpClient.GetAchievementAsync(regionName, achievementId, cancellationToken);
        var map = _mapper.Map<SelectedAchievementDto>(achievement);
        return Ok(map);
    }
}
