using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.WoWGameData;

[Route("api/v1/[controller]")]
[ApiController]
public class WoWAccountController(IWoWAccountService service) : ControllerBase
{
    private readonly IWoWAccountService _service = service;

    [HttpGet("getCharacters")]
    public async Task<IActionResult> GetCharacters(string regionName, CancellationToken cancellationToken)
    {
        var accountCharacters = await _service.GetCharactersAsync(regionName, cancellationToken);
        return Ok(accountCharacters);
    }

    [HttpGet("getCharactersList")]
    public async Task<IActionResult> GetCharactersList(string regionName, CancellationToken cancellationToken)
    {
        var accountCharactersList = await _service.GetCharactersListAsync(regionName, cancellationToken);
        return Ok(accountCharactersList);
    }

    [HttpGet("getMounts")]
    public async Task<IActionResult> GetMounts(string regionName, CancellationToken cancellationToken)
    {
        var mounts = await _service.GetAccountMountsAsync(regionName, cancellationToken);
        return Ok(mounts);
    }

    [HttpGet("getPets")]
    public async Task<IActionResult> GetPets(string regionName, CancellationToken cancellationToken)
    {
        var pets = await _service.GetAccountPetsAsync(regionName, cancellationToken);
        return Ok(pets);
    }

    [HttpGet("getToys")]
    public async Task<IActionResult> GetToys(string regionName, CancellationToken cancellationToken)
    {
        var toys = await _service.GetAccountToysAsync(regionName, cancellationToken);
        return Ok(toys);
    }

    [HttpGet("getSetTransmogs")]
    public async Task<IActionResult> GetSetTransmogs(string regionName, CancellationToken cancellationToken)
    {
        var setTransmogs = await _service.GetAccountSetTransmogsAsync(regionName, cancellationToken);
        return Ok(setTransmogs);
    }

    [HttpGet("getSlotTransmogs")]
    public async Task<IActionResult> GetSlotTransmogs(string regionName, CancellationToken cancellationToken)
    {
        var slotTransmogs = await _service.GetAccountSlotTransmogsAsync(regionName, cancellationToken);
        return Ok(slotTransmogs);
    }

    [HttpGet("getDashboard")]
    public async Task<IActionResult> GetDashboard(string regionName, string serverName, string characterName, CancellationToken cancellationToken)
    {
        var dashboard = await _service.GetAccountDashboardAsync(regionName, serverName, characterName, cancellationToken);
        return Ok(dashboard);
    }
}
