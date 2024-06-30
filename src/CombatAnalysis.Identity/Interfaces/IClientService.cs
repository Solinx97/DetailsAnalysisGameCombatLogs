using CombatAnalysis.Identity.DTO;

namespace CombatAnalysis.Identity.Interfaces;

public interface IClientService
{
    Task CreateAsync(ClientDto user);

    Task<ClientDto> GetAsymc(string id);
}
