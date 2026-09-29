using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account.Collections;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.WoWGameData;

[Route("api/v1/[controller]")]
[ApiController]
public class WoWDataController(IWoWGameDataApiClient httpClient, IMythicKeystoneService service, IMapper mapper) : ControllerBase
{
    private readonly IWoWGameDataApiClient _httpClient = httpClient;
    private readonly IMythicKeystoneService _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpGet("getRealms/{regionName}")]
    public async Task<IActionResult> GetRealms(string regionName, CancellationToken cancellationToken)
    {
        var realms = await _httpClient.GetRealmsAsync(regionName, cancellationToken);
        var map = _mapper.Map<WoWRealmDto[]>(realms.Realms);
        return Ok(map);
    }

    [HttpGet("getAchievement/{achievementId:int:min(1)}")]
    public async Task<IActionResult> GetAchievement(int achievementId, string regionName, CancellationToken cancellationToken)
    {
        var achievement = await _httpClient.GetAchievementAsync(regionName, achievementId, cancellationToken);
        var map = _mapper.Map<SelectedAchievementDto>(achievement);
        return Ok(map);
    }

    [HttpGet("getMount/{mountId:int:min(1)}")]
    public async Task<IActionResult> GetMount(int mountId, string regionName, CancellationToken cancellationToken)
    {
        var mount = await _httpClient.GetMountAsync(regionName, mountId, cancellationToken);
        var map = _mapper.Map<SelectedWoWAccountCollectionItemDto>(mount);
        return Ok(map);
    }

    [HttpGet("getPet/{petId:int:min(1)}")]
    public async Task<IActionResult> GetPet(int petId, string regionName, CancellationToken cancellationToken)
    {
        var pet = await _httpClient.GetPetAsync(regionName, petId, cancellationToken);
        var map = _mapper.Map<SelectedWoWAccountCollectionItemDto>(pet);
        return Ok(map);
    }

    [HttpGet("getMythicKeystoneLeaderboard/{connectedRealmId:int:min(1)}")]
    public async Task<IActionResult> GetMythicKeystoneLeaderboard(int connectedRealmId, int periodId, string regionName, CancellationToken cancellationToken)
    {
        var mythicKeystoneLeaderboard = await _service.GetMythicKeystoneLeaderboardAsync(connectedRealmId, periodId, regionName, cancellationToken);
        return Ok(mythicKeystoneLeaderboard);
    }

    [HttpGet("getWoWToken/{regionName}")]
    public async Task<IActionResult> GetWoWToken(string regionName, CancellationToken cancellationToken)
    {
        var token = await _httpClient.GetWoWTokenAsync(regionName, cancellationToken);
        var map = _mapper.Map<WoWTokenDto>(token);
        return Ok(map);
    }
}
