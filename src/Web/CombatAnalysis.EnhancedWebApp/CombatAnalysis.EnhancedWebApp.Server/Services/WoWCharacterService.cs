using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account.Collections;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Decors;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;

namespace CombatAnalysis.EnhancedWebApp.Server.Services;

internal class WoWCharacterService(IWoWGameDataApiClient httpClient, IWoWCharacterGameDataApiClient characterHttpClient, IMapper mapper) : IWoWCharacterService
{
    private readonly IWoWGameDataApiClient _httpClient = httpClient;
    private readonly IWoWCharacterGameDataApiClient _characterHttpClient = characterHttpClient;
    private readonly IMapper _mapper = mapper;

    public async Task<WoWAccountCollectionItemDto[]> GetDecorsAsync(string regionName, string serverName, string username, CancellationToken cancellationToken)
    {
        var allDecors = await _httpClient.GetDecorsAsync(regionName, cancellationToken);
        var characterDecors = await _characterHttpClient.GetDecorsAsync(serverName, username, regionName, cancellationToken);

        WoWAccountCollectionItemDto[] decors = [.. allDecors.Items.Select(x =>
        {
            var decor = characterDecors.DdecorCollected.FirstOrDefault(y => y.Decor.Id == x.Id);

            var result = new WoWAccountCollectionItemDto {
                Item = _mapper.Map<WoWGameDataEntityDto>(x),
                Info = decor != null
                    ? new WoWCharacterDecorInfoDto {
                        Quantity = decor.Quantity,
                    }
                    : null
            };

            return result;
        })];

        return decors;
    }
}
