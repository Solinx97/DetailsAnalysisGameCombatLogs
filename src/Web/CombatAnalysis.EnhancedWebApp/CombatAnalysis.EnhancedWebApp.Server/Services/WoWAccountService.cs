using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;

namespace CombatAnalysis.EnhancedWebApp.Server.Services;

public class WoWAccountService(IWoWGameDataApiClient httpClient, IWoWAccountGameDataApiClient accountHttpClient, IMapper mapper) : IWoWAccountService
{
    private readonly IWoWGameDataApiClient _httpClient = httpClient;
    private readonly IWoWAccountGameDataApiClient _accountHttpClient = accountHttpClient;
    private readonly IMapper _mapper = mapper;

    public async Task<WoWAccountCollectionItemDto[]> GetAccountMountsAsync(string regionName, CancellationToken cancellationToken)
    {
        var allMounts = await _httpClient.GetMountsAsync(regionName, cancellationToken);
        var acoountMounts = await _accountHttpClient.GetMountsAsync(regionName, cancellationToken);

        WoWAccountCollectionItemDto[] mounts = [.. allMounts.Mounts.Select(x =>
        {
            var mount = acoountMounts.Mounts.FirstOrDefault(y => y.Mount.Id == x.Id);

            var result = new WoWAccountCollectionItemDto {
                Item = _mapper.Map<WoWGameDataEntityDto>(x),
                Info = mount != null
                    ? new WoWAccountCollectionItemInfoDto {
                        IsFavorite =  mount.IsFavorite,
                    }
                    : null
            };

            return result;
        })];

        return mounts;
    }

    public async Task<WoWAccountCollectionItemDto[]> GetAccountPetsAsync(string regionName, CancellationToken cancellationToken)
    {
        var allPets = await _httpClient.GetPetsAsync(regionName, cancellationToken);
        var accountPets = await _accountHttpClient.GetPetsAsync(regionName, cancellationToken);

        WoWAccountCollectionItemDto[] pets = [.. allPets.Pets.Select(x =>
        {
            var pet = accountPets.Pets.FirstOrDefault(y => y.Species.Id == x.Id);

            var result = new WoWAccountCollectionItemDto {
                Item = _mapper.Map<WoWGameDataEntityDto>(x),
                Info = pet != null
                    ? new WoWAccountPetInfoDto {
                        Id = pet.Id,
                        Level = pet.Level,
                        Quality = _mapper.Map<WoWGameDataTypeDto>(pet.Quality),
                        Stats = _mapper.Map<WoWAccountPetStatDto>(pet.Stats),
                        IsFavorite = pet.IsFavorite,
                    }
                    : null
            };

            return result;
        })];

        return pets;
    }
}
