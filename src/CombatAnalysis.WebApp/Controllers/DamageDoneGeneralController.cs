using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CombatAnalysis.WebApp.Controllers;

[Route("[controller]")]
[ApiController]
public class DamageDoneGeneralController : ControllerBase
{
    private readonly IHttpClientHelper _httpClient;

    public DamageDoneGeneralController(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = Port.CombatParserApi;
    }

    [HttpGet("{id}")]
    public async Task<IEnumerable<DamageDoneGeneralModel>> GetById(int id)
    {
        var responseMessage = await _httpClient.GetAsync($"DamageDoneGeneral/FindByCombatPlayerId/{id}");
        var damageDoneGenerals = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageDoneGeneralModel>>();

        return damageDoneGenerals;
    }
}
