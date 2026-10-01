using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account.Collections;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Equipments;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Professions;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Reputation;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Professions;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Reputation;

namespace CombatAnalysis.EnhancedWebApp.Server.Mapping;

public class ProxyApiMapper : Profile
{
    public ProxyApiMapper()
    {
        CreateMap<WoWGameDataEntityModel, WoWGameDataEntityDto>();
        CreateMap<WoWGameDataColorModel, WoWGameDataColorDto>();
        CreateMap<WoWGameDataCurrencyDisplayModel, WoWGameDataCurrencyDisplayDto>();
        CreateMap<WoWGameDataCurrencyModel, WoWGameDataCurrencyDto>();
        CreateMap<WoWGameDataPlayableClassModel, WoWGameDataPlayableClassDto>();
        CreateMap<WoWGameDataValueModel, WoWGameDataValueDto>();
        CreateMap<WoWGameDataTypeModel, WoWGameDataTypeDto>();
        CreateMap<WoWGameDataItemDisplayModel, WoWGameDataItemDisplayDto>();

        CreateMap<CharacterReputationModel, CharacterReputationDto>();
        CreateMap<CharacterReputationStandingModel, CharacterReputationStandingDto>();

        CreateMap<WoWAccountMountModel, WoWAccountCollectionItemDto>();

        CreateMap<CharacterSummaryModel, CharacterSummaryDto>();

        CreateMap<CharacterClassModel, CharacterClassDto>();
        CreateMap<CharacterSpecializationModel, CharacterSpecializationDto>();
        CreateMap<CharacterGenderModel, CharacterGenderDto>();
        CreateMap<CharacterTitleModel, CharacterTitleDto>();
        CreateMap<WoWRealmModel, WoWRealmDto>();
        CreateMap<FactionModel, FactionDto>();
        CreateMap<GuildModel, GuildDto>();

        CreateMap<CharacterEquipmentCraftingStatModel, CharacterEquipmentCraftingStatDto>();
        CreateMap<CharacterEquipmentModel, CharacterEquipmentDto>();
        CreateMap<CharacterEquipmentEnchantmentModel, CharacterEquipmentEnchantmentDto>();
        CreateMap<CharacterEquipmentRequirementsModel, CharacterEquipmentRequirementsDto>();
        CreateMap<CharacterEquipmentSetModel, CharacterEquipmentSetDto>();
        CreateMap<CharacterEquipmentSetEffectModel, CharacterEquipmentSetEffectDto>();
        CreateMap<CharacterEquipmentSetItemModel, CharacterEquipmentSetItemDto>();
        CreateMap<CharacterEquipmentSlotModel, CharacterEquipmentSlotDto>();
        CreateMap<CharacterEquipmentSocketModel, CharacterEquipmentSocketDto>();
        CreateMap<CharacterEquipmentSpellModel, CharacterEquipmentSpellDto>();
        CreateMap<CharacterEquipmentsResponse, CharacterEquipmentsResponseDto>();
        CreateMap<CharacterEquipmentStatModel, CharacterEquipmentStatDto>();
        CreateMap<CharacterEquipmentTransmogModel, CharacterEquipmentTransmogDto>();

        CreateMap<CharacterStatsModel, CharacterStatsDto>();
        CreateMap<CharacterStatPowerRatingModel, CharacterStatPowerRatingDto>();
        CreateMap<CharacterStatPowerModel, CharacterStatPowerDto>();

        CreateMap<MythicKeystoneModel, MythicKeystoneDto>()
            .ForMember(
                dest => dest.Seasons,
                opt => opt.MapFrom(src => src.Seasons.OrderBy(x => x.Id).ToList()));
        CreateMap<MythicKeystoneCurrentPeriodModel, MythicKeystoneCurrentPeriodDto>();
        CreateMap<WoWGameDataCharacterModel, WoWGameDataCharacterDto>();
        CreateMap<MythicKeystoneRaitingModel, MythicKeystoneRaitingDto>();
        CreateMap<MythicKeystoneMemberModel, MythicKeystoneMemberDto>();
        CreateMap<MythicKeystoneBestRunModel, MythicKeystoneBestRunDto>();

        CreateMap<MythicKeystoneSeasonModel, MythicKeystoneSeasonDto>();

        CreateMap<MythicKeystoneDungeonLeaderboardAfixModel, MythicKeystoneDungeonLeaderboardAfixDto>();
        CreateMap<MythicKeystoneDungeonLeaderboardGroupMemberModel, MythicKeystoneDungeonLeaderboardGroupMemberDto>();
        CreateMap<MythicKeystoneDungeonLeaderboardGroupModel, MythicKeystoneDungeonLeaderboardGroupDto>()
            .ForMember(
                dest => dest.Duration,
                opt => opt.MapFrom(src => TimeSpan.FromMilliseconds(src.Duration)))
            .ForMember(
                dest => dest.CompletedTime,
                opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(src.CompletedTimestamp)));
        CreateMap<MythicKeystoneDungeonLeaderboardModel, MythicKeystoneDungeonLeaderboardDto>()
            .ForMember(
                dest => dest.PeriodStartTime,
                opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(src.PeriodStartTimestamp)))
            .ForMember(
                dest => dest.PeriodEndTime,
                opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(src.PeriodEndTimestamp)));

        CreateMap<CharacterDungeonModel, CharacterDungeonDto>();
        CreateMap<DungeonExpansionModel, DungeonExpansionDto>();
        CreateMap<DungeonInstanceModel, DungeonInstanceDto>();
        CreateMap<DungeonModeEncountModel, DungeonModeEncountDto>()
            .ForMember(
                dest => dest.LastKillTime,
                opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(src.LastKillTimestamp)));
        CreateMap<DungeonModeModel, DungeonModeDto>();
        CreateMap<DungeonModeProgressModel, DungeonModeProgressDto>();

        CreateMap<CharacterAchievementCategoryModel, CharacterAchievementCategoryDto>();
        CreateMap<CharacterAchievementCriteriaModel, CharacterAchievementCriteriaDto>();
        CreateMap<CharacterAchievementModel, CharacterAchievementDto>()
            .ForMember(
                dest => dest.CompletedTime,
                opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(src.CompletedTimestamp)));
        CreateMap<CharacterAchievementRecentEventsModel, CharacterAchievementRecentEventsDto>()
            .ForMember(
                dest => dest.Time,
                opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(src.Timestamp)));
        CreateMap<CharacterAchievementsModel, CharacterAchievementsDto>();
        CreateMap<AchievementCategoriesModel, AchievementCategoriesDto>();

        CreateMap<CharacterAchievementStatisticsCategoryModel, CharacterAchievementStatisticsCategoryDto>();
        CreateMap<CharacterAchievementStatisticsSubCategoryModel, CharacterAchievementStatisticsSubCategoryDto>();
        CreateMap<CharacterAchievementStatisticModel, CharacterAchievementStatisticDto>()
            .ForMember(
                dest => dest.LastUpdatedTime,
                opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(src.LastUpdatedTimestamp)));

        CreateMap<AchievementSelectedCategoryModel, AchievementSelectedCategoryDto>();
        CreateMap<AchievementSelectedCategoryFactionModel, AchievementSelectedCategoryFactionDto>();
        CreateMap<AchievementCategoryFactionModel, AchievementCategoryFactionDto>();
        CreateMap<AchievementCategoryModel, AchievementCategoryDto>();
        CreateMap<AchievementExtendModel, AchievementExtendDto>();

        CreateMap<SelectedAchievementModel, SelectedAchievementDto>();
        CreateMap<SelectedAchievementCriteriaModel, SelectedAchievementCriteriaDto>();

        CreateMap<WoWRealmModel, WoWRealmDto>();

        CreateMap<SelectedWoWAccountCollectionItemModel, SelectedWoWAccountCollectionItemDto>();
        CreateMap<SelectedWoWAccountToyItemModel, SelectedWoWAccountToyItemDto>();

        CreateMap<WoWAccountRespone, WoWAccountResponseDto>();
        CreateMap<WoWAccountModel, WoWAccountDto>()
            .ForMember(
                dest => dest.Characters,
                opt => opt.MapFrom(src =>
                    src.Characters.GroupBy(x => x.Realm.Name!)
                                    .ToDictionary(
                                        g => g.Key,
                                        g => g.ToArray())));
        CreateMap<CharacterModel, CharacterDto>();

        CreateMap<CharacterProfessionsResponse, CharacterProfessionsResponseDto>();
        CreateMap<CharacterProfessionTierModel, CharacterProfessionTierDto>();
        CreateMap<CharacterProfessionModel, CharacterProfessionDto>();

        CreateMap<WoWTokenModel, WoWTokenDto>()
            .ForMember(
                dest => dest.LastUpdatedTime,
                opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(src.LastUpdatedTimestamp)));

        CreateMap<WoWAccountPetStatModel, WoWAccountPetStatDto>();
    }
}
