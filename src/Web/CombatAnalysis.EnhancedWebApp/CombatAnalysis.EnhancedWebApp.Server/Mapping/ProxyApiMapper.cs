using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Reputation;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Reputation;

namespace CombatAnalysis.EnhancedWebApp.Server.Mapping;

public class ProxyApiMapper : Profile
{
    public ProxyApiMapper()
    {
        CreateMap<CharacterReputationModel, CharacterReputationDto>();
        CreateMap<CharacterReputationFactionModel, CharacterReputationFactionDto>();
        CreateMap<CharacterReputationStandingModel, CharacterReputationStandingDto>();

        CreateMap<CharacterMountModel, CharacterMountDto>();
        CreateMap<MountModel, MountDto>();

        CreateMap<CharacterModel, CharacterDto>();

        CreateMap<CharacterRaceModel, CharacterRaceDto>();
        CreateMap<CharacterClassModel, CharacterClassDto>();
        CreateMap<CharacterSpecializationModel, CharacterSpecializationDto>();
        CreateMap<CharacterGenderModel, CharacterGenderDto>();
        CreateMap<CharacterTitleModel, CharacterTitleDto>();
        CreateMap<RealmModel, RealmDto>();
        CreateMap<FactionModel, FactionDto>();
        CreateMap<GuildModel, GuildDto>();

        CreateMap<MythicKeystoneModel, MythicKeystoneDto>();
        CreateMap<MythicKeystoneCurrentPeriodModel, MythicKeystoneCurrentPeriodDto>();
        CreateMap<MythicKeystoneSeasonModel, MythicKeystoneSeasonDto>();
        CreateMap<DungeonCharacterModel, DungeonCharacterDto>();
        CreateMap<MythicKeystoneRaitingModel, MythicKeystoneRaitingDto>();
        CreateMap<MythicKeystoneAfixModel, MythicKeystoneAfixDto>();
        CreateMap<MythicKeystoneMemberModel, MythicKeystoneMemberDto>();
        CreateMap<MythicKeystoneBestRunModel, MythicKeystoneBestRunDto>();
        CreateMap<MythicKeystoneDungeonModel, MythicKeystoneDungeonDto>();
        CreateMap<MythicKeystoneColorModel, MythicKeystoneColorDto>();

        CreateMap<CharacterDungeonModel, CharacterDungeonDto>();
        CreateMap<DungeonExpansionModel, DungeonExpansionDto>();
        CreateMap<DungeonInstanceModel, DungeonInstanceDto>();
        CreateMap<DungeonModeEncountModel, DungeonModeEncountDto>()
            .ForMember(
                dest => dest.LastKillTime,
                opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(src.LastKillTimestamp)));
        CreateMap<DungeonModeModel, DungeonModeDto>();
        CreateMap<DungeonModeProgressModel, DungeonModeProgressDto>();
        CreateMap<DungeonModeTypeModel, DungeonModeTypeDto>();
        CreateMap<DungeonModel, DungeonDto>();

        CreateMap<CharacterAchievementCategoryModel, CharacterAchievementCategoryDto>();
        CreateMap<CharacterAchievementCriteriaModel, CharacterAchievementCriteriaDto>();
        CreateMap<CharacterAchievementModel, CharacterAchievementDto>()
            .ForMember(
                dest => dest.CompletedTime,
                opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(src.CompletedTimestamp)));
        CreateMap<AchievementModel, AchievementDto>();
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
    }
}
