using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.WoWGameData;

[Route("api/v1/[controller]")]
[ApiController]
public class WoWAccountController(IWoWAccountService service, IWoWAccountGameDataApiClient accountHttpClient, IMapper mapper) : ControllerBase
{
    private readonly IWoWAccountService _service = service;
    private readonly IWoWAccountGameDataApiClient _accountHttpClient = accountHttpClient;
    private readonly IMapper _mapper = mapper;

    [HttpGet("getCharacters")]
    public async Task<IActionResult> GetCharacters(string regionName, CancellationToken cancellationToken)
    {
        var accountCharacters = await _accountHttpClient.GetCharactersAsync(regionName, cancellationToken);
        var map = _mapper.Map<WoWAccountResponseDto>(accountCharacters);
        return Ok(map);
    }

    [HttpGet("getMounts")]
    public async Task<IActionResult> GetMounts(string regionName, CancellationToken cancellationToken)
    {
        var mounts = await _service.GetAccountMountsAsync(regionName, cancellationToken);
        var map = _mapper.Map<MountDto[]>(mounts);
        return Ok(map);
    }
}
