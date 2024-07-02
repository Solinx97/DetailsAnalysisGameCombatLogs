using AutoMapper;
using CombatAnalysis.Identity.DTO;
using CombatAnalysisIdentity.Models;

namespace CombatAnalysisIdentity.Mapping;

public class CombatAnalysisIdentityMapper : Profile
{
    public CombatAnalysisIdentityMapper()
    {
        CreateMap<IdentityUserDto, IdentityUserModel>().ReverseMap();
    }
}
