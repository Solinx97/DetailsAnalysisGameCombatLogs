using AutoMapper;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.Identity.DTO;

namespace CombatAnalysis.Identity.Mapping
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile()
        {
            CreateMap<RefreshTokenDto, RefreshToken>().ReverseMap();
        }
    }
}
