using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account.Collections;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

namespace CombatAnalysis.EnhancedWebApp.Server.Services;

internal class WoWAccountService(IWoWGameDataApiClient httpClient, IWoWCharacterGameDataApiClient characterHttpClient, IWoWAccountGameDataApiClient accountHttpClient, IWoWCharacterService service, IMapper mapper) : IWoWAccountService
{
    private const int EXPANSION_MAX_LEVEL = 90;

    private readonly IWoWGameDataApiClient _httpClient = httpClient;
    private readonly IWoWCharacterGameDataApiClient _characterHttpClient = characterHttpClient;
    private readonly IWoWAccountGameDataApiClient _accountHttpClient = accountHttpClient;
    private readonly IWoWCharacterService _service = service;
    private readonly IMapper _mapper = mapper;

    public async Task<WoWAccountResponseDto> GetCharactersAsync(string regionName, CancellationToken cancellationToken)
    {
        var accountCharacters = await _accountHttpClient.GetCharactersAsync(regionName, cancellationToken);
        var map = _mapper.Map<WoWAccountResponseDto>(accountCharacters);
        foreach (var account in map.WowAccounts)
        {
            account.Characters = account.Characters.ToDictionary(x => x.Key, x => x.Value.OrderByDescending(c => c.Level).ToArray());
        }

        return map;
    }

    public async Task<CharacterModel[]> GetCharactersListAsync(string regionName, CancellationToken cancellationToken)
    {
        var accountCharacters = await _accountHttpClient.GetCharactersAsync(regionName, cancellationToken);
        var characters = accountCharacters.WowAccounts.SelectMany(x => x.Characters).OrderByDescending(x => x.Level).ToArray();

        return characters;
    }

    public async Task<WoWAccountCollectionItemDto[]> GetAccountMountsAsync(string regionName, CancellationToken cancellationToken)
    {
        var allMounts = await _httpClient.GetMountsAsync(regionName, cancellationToken);
        var acoountMounts = await _accountHttpClient.GetMountsAsync(regionName, cancellationToken);

        WoWAccountCollectionItemDto[] mounts = [.. allMounts.Items.Select(x =>
        {
            var mount = acoountMounts.Mounts.FirstOrDefault(y => y.Item.Id == x.Id);

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

        var orderByFavorite = mounts.OrderByDescending(x => x.Info != null && x.Info.IsFavorite).ToArray();
        return orderByFavorite;
    }

    public async Task<WoWAccountCollectionItemDto[]> GetAccountToysAsync(string regionName, CancellationToken cancellationToken)
    {
        var allToys = await _httpClient.GetToysAsync(regionName, cancellationToken);
        var acoountToys = await _accountHttpClient.GetToysAsync(regionName, cancellationToken);

        WoWAccountCollectionItemDto[] toys = [.. allToys.Items.Select(x =>
        {
            var toy = acoountToys.Toys.FirstOrDefault(y => y.Item.Id == x.Id);

            var result = new WoWAccountCollectionItemDto {
                Item = _mapper.Map<WoWGameDataEntityDto>(x),
                Info = toy != null
                    ? new WoWAccountCollectionItemInfoDto {
                        IsFavorite =  toy.IsFavorite,
                    }
                    : null
            };

            return result;
        })];

        var orderByFavorite = toys.OrderByDescending(x => x.Info != null && x.Info.IsFavorite).ToArray();
        return orderByFavorite;
    }

    public async Task<WoWAccountCollectionItemDto[]> GetAccountPetsAsync(string regionName, CancellationToken cancellationToken)
    {
        var allPets = await _httpClient.GetPetsAsync(regionName, cancellationToken);
        var accountPets = await _accountHttpClient.GetPetsAsync(regionName, cancellationToken);

        WoWAccountCollectionItemDto[] pets = [.. allPets.Items.Select(x =>
        {
            var pet = accountPets.Pets.FirstOrDefault(y => y.Item.Id == x.Id);

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

        var orderByFavorite = pets.OrderByDescending(x => x.Info != null && x.Info.IsFavorite).ToArray();
        return orderByFavorite;
    }

    public async Task<WoWAccountCollectionItemDto[]> GetAccountSetTransmogsAsync(string regionName, CancellationToken cancellationToken)
    {
        var allSetTransmogs = await _httpClient.GetSetsTransmogsAsync(regionName, cancellationToken);
        var acoountTransmogs = await _accountHttpClient.GetTransmogsAsync(regionName, cancellationToken);

        WoWAccountCollectionItemDto[] setTransmogs = [.. allSetTransmogs.Items.Select(x =>
        {
            var transmog = acoountTransmogs.AppearanceSets.FirstOrDefault(y => y.Id == x.Id);

            var result = new WoWAccountCollectionItemDto {
                Item = _mapper.Map<WoWGameDataEntityDto>(x),
                Info = transmog != null
                    ? new WoWAccountCollectionItemInfoDto()
                    : null
            };

            return result;
        })];

        return setTransmogs;
    }

    public async Task<Dictionary<string, WoWAccountCollectionItemDto[]>> GetAccountSlotTransmogsAsync(string regionName, CancellationToken cancellationToken)
    {
        var result = new Dictionary<string, WoWAccountCollectionItemDto[]>();

        var acoountTransmogs = await _accountHttpClient.GetTransmogsAsync(regionName, cancellationToken);
        var slots = acoountTransmogs.Slots.Select(x => x.Slot.Type).ToList();
        foreach (var item in slots)
        {
            var slotTransmogs = await GetAccountSlotTransmogsByTypeAsync(acoountTransmogs, regionName, item, cancellationToken);
            result.TryAdd(item, slotTransmogs);
        }

        return result;
    }

    private async Task<WoWAccountCollectionItemDto[]> GetAccountSlotTransmogsByTypeAsync(WoWAccountTransmogResponse acoountTransmogs, string regionName, string slotType, CancellationToken cancellationToken)
    {
        var allSlotTransmogs = await _httpClient.GetSlotTransmogsAsync(regionName, slotType, cancellationToken);

        WoWAccountCollectionItemDto[] slotTransmogs = [.. allSlotTransmogs.Items.Select(x =>
        {
            var transmogSlot = acoountTransmogs.Slots.FirstOrDefault(y => y.Slot.Type == slotType);
            if (transmogSlot == null) {
                return new WoWAccountCollectionItemDto {
                    Item = _mapper.Map<WoWGameDataEntityDto>(x)
                };
            }

            var transmog = transmogSlot.Appearances.FirstOrDefault(y => y.Id == x.Id);

            var result = new WoWAccountCollectionItemDto {
                Item = _mapper.Map<WoWGameDataEntityDto>(x),
                Info = transmog != null
                    ? new WoWAccountCollectionItemInfoDto()
                    : null
            };

            return result;
        })];

        return slotTransmogs;
    }

    public async Task<AccountDetailsDto> GetAccountDashboardAsync(string regionName, string serverName, string characterName, CancellationToken cancellationToken)
    {
        var characterSummary = await _characterHttpClient.GetProfileSummaryAsync(serverName, characterName, regionName, cancellationToken);

        var mythicKeystone = await _characterHttpClient.GetMythicKeystoneAsync(serverName, characterName, regionName, cancellationToken);

        var allAchievements = await _httpClient.GetAchievementsAsync(regionName, cancellationToken);
        var characterAchievements = await _characterHttpClient.GetAchievementsAsync(serverName, characterName, regionName, cancellationToken);

        var allMounts = await _httpClient.GetMountsAsync(regionName, cancellationToken);
        var acoountMounts = await _accountHttpClient.GetMountsAsync(regionName, cancellationToken);

        var allToys = await _httpClient.GetToysAsync(regionName, cancellationToken);
        var acoountToys = await _accountHttpClient.GetToysAsync(regionName, cancellationToken);

        var allPets = await _httpClient.GetPetsAsync(regionName, cancellationToken);
        var accountPets = await _accountHttpClient.GetPetsAsync(regionName, cancellationToken);

        var allSetTransmogs = await _httpClient.GetSetsTransmogsAsync(regionName, cancellationToken);
        var acoountTransmogs = await _accountHttpClient.GetTransmogsAsync(regionName, cancellationToken);

        var slotsTransmogs = await GetAccountSlotTransmogsAsync(regionName, cancellationToken);

        var accountCharacters = await _accountHttpClient.GetCharactersAsync(regionName, cancellationToken);

        var characters = accountCharacters.WowAccounts.SelectMany(x => x.Characters);;

        var allDecors = await _httpClient.GetDecorsAsync(regionName, cancellationToken);
        var decors = await _service.GetDecorsAsync(regionName, serverName, characterName, cancellationToken);

        var details = new AccountDetailsDto
        {
            Summary = _mapper.Map<CharacterSummaryDto>(characterSummary),
            MythicKeystoneRating = mythicKeystone.CurrentMythicRating == null ? 0 : mythicKeystone.CurrentMythicRating.Rating,
            AchievementsReceived = characterAchievements.TotalQuantity,
            AchievementsCount = allAchievements.Achievements.Length,
            MountsReceived = acoountMounts.Mounts.Length,
            MountsCount = allMounts.Items.Length,
            ToysReceived = acoountToys.Toys.Length,
            ToysCount = allToys.Items.Length,
            PetsReceived = accountPets.Pets.Length,
            PetsCount = allPets.Items.Length,
            DecorsReceived = decors.Length,
            DecorsCount = allDecors.Items.Length,
            SetTransmogsReceived = acoountTransmogs.AppearanceSets.Length,
            SetTransmogsCount = allSetTransmogs.Items.Length,
            TransmogsReceived = slotsTransmogs.Sum(x => x.Value.Where(y => y.Info != null).Count()),
            TransmogsCount = slotsTransmogs.Sum(x => x.Value.Length),
            CharactersCount = characters.Count(),
            MaxLevelCharactersCount = characters.Where(x => x.Level == EXPANSION_MAX_LEVEL).Count()
        };

        return details;
    }
}
