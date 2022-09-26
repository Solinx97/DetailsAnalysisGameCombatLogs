using AutoMapper;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.Identity.DTO;

namespace CombatAnalysis.Identity.Mapping
{
    public class IdentityMappingMapper : Profile
    {
        public IdentityMappingMapper()
        {
            CreateMap<RefreshTokenDto, RefreshToken>().ReverseMap();
        }
    }
}
