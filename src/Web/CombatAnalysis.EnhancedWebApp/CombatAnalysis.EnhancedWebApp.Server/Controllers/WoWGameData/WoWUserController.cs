using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.WoWGameData;

[Route("api/v1/[controller]")]
[ApiController]
public class WoWUserController(IWoWUserGameDataApiClient httpClient, IMapper mapper) : ControllerBase
{
    private readonly IWoWUserGameDataApiClient _httpClient = httpClient;
    private readonly IMapper _mapper = mapper;

    [HttpGet("getMounts")]
    public async Task<IActionResult> GetMounts(string regionName, CancellationToken cancellationToken)
    {
        var mounts = await _httpClient.GetMountsAsync(regionName, cancellationToken);
        var map = _mapper.Map<CharacterMountDto[]>(mounts.Mounts);
        return Ok(map);
    }
}
