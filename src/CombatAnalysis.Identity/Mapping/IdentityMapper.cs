using AutoMapper;
using CombatAnalysis.Identity.DTO;
using CombatAnalysis.IdentityDAL.Entities;

namespace CombatAnalysis.Identity.Mapping;

public class IdentityMapper : Profile
{
    public IdentityMapper()
    {
        CreateMap<IdentityUserDto, IdentityUser>().ReverseMap();
        CreateMap<ClientDto, Client>().ReverseMap();
    }
}
