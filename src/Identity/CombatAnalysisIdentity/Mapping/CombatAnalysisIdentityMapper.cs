using AutoMapper;
using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.Identity.DTO;
using CombatAnalysisIdentity.Models;

namespace CombatAnalysisIdentity.Mapping;

public class CombatAnalysisIdentityMapper : Profile
{
    public CombatAnalysisIdentityMapper()
    {
        CreateMap<IdentityUserDto, IdentityUserModel>().ReverseMap();
        CreateMap<AppUserDto, AppUserModel>().ReverseMap();
        CreateMap<CustomerDto, CustomerModel>().ReverseMap();
    }
}
