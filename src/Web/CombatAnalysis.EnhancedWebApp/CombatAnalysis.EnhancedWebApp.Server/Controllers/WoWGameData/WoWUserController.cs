using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.EnhancedWebApp.Server.Controllers.WoWGameData;

[Route("api/v1/[controller]")]
[ApiController]
public class WoWUserController(IMountService service, IMapper mapper) : ControllerBase
{
    private readonly IMountService _service = service;
    private readonly IMapper _mapper = mapper;

    [HttpGet("getMounts")]
    public async Task<IActionResult> GetMounts(string regionName, CancellationToken cancellationToken)
    {
        var mounts = await _service.GetUserMountsAsync(regionName, cancellationToken);
        var map = _mapper.Map<MountDto[]>(mounts);
        return Ok(map);
    }
}
