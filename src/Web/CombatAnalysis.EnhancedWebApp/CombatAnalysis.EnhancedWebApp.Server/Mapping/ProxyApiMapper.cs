using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Reputation;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character;
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
        CreateMap<CharacterReputationFactionModel, CharacterReputationFactionDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.Name));
        CreateMap<CharacterReputationStandingModel, CharacterReputationStandingDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.Name));

        CreateMap<CharacterMountModel, CharacterMountDto>();
        CreateMap<MountModel, MountDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.Name));

        CreateMap<CharacterModel, CharacterDto>();

        CreateMap<CharacterRaceModel, CharacterRaceDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.Name));
        CreateMap<CharacterClassModel, CharacterClassDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.Name));
        CreateMap<CharacterSpecializationModel, CharacterSpecializationDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.Name));
        CreateMap<CharacterGenderModel, CharacterGenderDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.Name));
        CreateMap<CharacterTitleModel, CharacterTitleDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.Name))
            .ForMember(
                dest => dest.DisplayString,
                opt => opt.MapFrom(src => src.DisplayString.Name));
        CreateMap<RealmModel, RealmDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.Name));
        CreateMap<FactionModel, FactionDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.Name));
        CreateMap<GuildModel, GuildDto>();

        CreateMap<MythicKeystoneModel, MythicKeystoneDto>();
        CreateMap<MythicKeystoneCurrentPeriodModel, MythicKeystoneCurrentPeriodDto>();
        CreateMap<MythicKeystoneSeasonModel, MythicKeystoneSeasonDto>();
        CreateMap<DungeonCharacterModel, DungeonCharacterDto>();
        CreateMap<MythicKeystoneRaitingModel, MythicKeystoneRaitingDto>();
        CreateMap<MythicKeystoneAfixModel, MythicKeystoneAfixDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.Name));
        CreateMap<MythicKeystoneMemberModel, MythicKeystoneMemberDto>();
        CreateMap<MythicKeystoneBestRunModel, MythicKeystoneBestRunDto>();
        CreateMap<MythicKeystoneDungeonModel, MythicKeystoneDungeonDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.Name));
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
        CreateMap<DungeonModeTypeModel, DungeonModeTypeDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.Name));
        CreateMap<DungeonNameModel, DungeonNameDto>();
    }
}
