using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Collections;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Reputation;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;
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
        CreateMap<WoWMountModel, WoWMountDto>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name.Name));
    }
}
