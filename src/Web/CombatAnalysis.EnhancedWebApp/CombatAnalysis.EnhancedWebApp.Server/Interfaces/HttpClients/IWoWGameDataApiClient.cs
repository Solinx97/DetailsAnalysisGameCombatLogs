using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Data;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IWoWGameDataApiClient
{
    Task<RealmsResponse> GetRealmsAsync(string regionName, CancellationToken cancellationToken);

    Task<SearchItemResponse> SearchItemAsync(string regionName, string name, string orderBy, int page, CancellationToken cancellationToken);

    Task<IWoWCollectionResponse> GetMountsAsync(string regionName, CancellationToken cancellationToken);

    Task<SelectedWoWAccountCollectionItemModel> GetMountAsync(string regionName, int mountId, CancellationToken cancellationToken);

    Task<IWoWCollectionResponse> GetPetsAsync(string regionName, CancellationToken cancellationToken);

    Task<SelectedWoWAccountCollectionItemModel> GetPetAsync(string regionName, int petId, CancellationToken cancellationToken);

    Task<IWoWCollectionResponse> GetToysAsync(string regionName, CancellationToken cancellationToken);

    Task<SelectedWoWAccountToyItemModel> GetToyAsync(string regionName, int toyId, CancellationToken cancellationToken);

    Task<IWoWCollectionResponse> GetSetsTransmogsAsync(string regionName, CancellationToken cancellationToken);

    Task<IWoWCollectionResponse> GetSlotTransmogsAsync(string regionName, string slotType, CancellationToken cancellationToken);

    Task<SelectedWoWAccountCollectionItemModel> GetTransmogAsync(string regionName, int transmogId, CancellationToken cancellationToken);

    Task<IWoWCollectionResponse> GetDecorsAsync(string regionName, CancellationToken cancellationToken);

    Task<SelectedWoWAccountCollectionItemModel> GetDecorAsync(string regionName, int decorId, CancellationToken cancellationToken);

    Task<AchievementCategoriesModel> GetAchievementCategoryAsync(string regionName, CancellationToken cancellationToken);

    Task<AchievementSelectedCategoryModel> GetAchievementCategoryAsync(string regionName, int categoryId, CancellationToken cancellationToken);

    Task<SelectedAchievementModel> GetAchievementAsync(string regionName, int achievementId, CancellationToken cancellationToken);

    Task<MythicKeystoneLeaderboardModel> GetMythicKeystoneLeaderboardAsync(int connectedRealmId, string regionName, CancellationToken cancellationToken);

    Task<MythicKeystoneDungeonLeaderboardModel> GetMythicDungeonKeystoneLeaderboardAsync(int connectedRealmId, long dungeonId, int periodId, string regionName, CancellationToken cancellationToken);

    Task<WoWTokenModel> GetWoWTokenAsync(string regionName, CancellationToken cancellationToken);

    Task<AuctionResponse> GetAuctionHouseCommoditiesAsync(string regionName, CancellationToken cancellationToken);
}
