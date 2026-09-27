using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Equipments;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Reputation;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;
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

        CreateMap<AccountMountModel, CharacterMountDto>();
        CreateMap<MountModel, MountDto>();

        CreateMap<CharacterSummaryModel, CharacterSummaryDto>();

        CreateMap<CharacterRaceModel, CharacterRaceDto>();
        CreateMap<CharacterClassModel, CharacterClassDto>();
        CreateMap<CharacterSpecializationModel, CharacterSpecializationDto>();
        CreateMap<CharacterGenderModel, CharacterGenderDto>();
        CreateMap<CharacterTitleModel, CharacterTitleDto>();
        CreateMap<RealmModel, RealmDto>();
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

        CreateMap<MythicKeystoneModel, MythicKeystoneDto>();
        CreateMap<MythicKeystoneCurrentPeriodModel, MythicKeystoneCurrentPeriodDto>();
        CreateMap<DungeonCharacterModel, DungeonCharacterDto>();
        CreateMap<MythicKeystoneRaitingModel, MythicKeystoneRaitingDto>();
        CreateMap<MythicKeystoneMemberModel, MythicKeystoneMemberDto>();
        CreateMap<MythicKeystoneBestRunModel, MythicKeystoneBestRunDto>();
        CreateMap<WoWGameDataColorModel, MythicKeystoneColorDto>();

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

        CreateMap<AchievementSelectedCategoryModel, AchievementSelectedCategoryDto>();
        CreateMap<AchievementSelectedCategoryFactionModel, AchievementSelectedCategoryFactionDto>();
        CreateMap<AchievementCategoryFactionModel, AchievementCategoryFactionDto>();
        CreateMap<AchievementCategoryModel, AchievementCategoryDto>();
        CreateMap<AchievementExtendModel, AchievementExtendDto>();

        CreateMap<SelectedAchievementModel, SelectedAchievementDto>();
        CreateMap<SelectedAchievementCriteriaModel, SelectedAchievementCriteriaDto>();

        CreateMap<RealmModel, RealmDto>();

        CreateMap<SelectedMountModel, SelectedMountDto>();

        CreateMap<WoWAccountRespone, WoWAccountResponseDto>();
        CreateMap<WoWAccountModel, WoWAccountDto>()
            .ForMember(
                dest => dest.Characters,
                opt => opt.MapFrom(src =>
                    src.Characters.GroupBy(x => x.Realm.Name)
                                    .ToDictionary(
                                        g => g.Key,
                                        g => g.ToArray())));
        CreateMap<CharacterModel, CharacterDto>();
    }
}
