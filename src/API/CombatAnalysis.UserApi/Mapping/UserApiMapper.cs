using AutoMapper;
using CombatAnalysis.BL.DTO.User;
using CombatAnalysis.UserApi.Models.User;

namespace CombatAnalysis.UserApi.Mapping
{
    public class UserApiMapper : Profile
    {
        public UserApiMapper()
        {
            CreateMap<UserDto, UserModel>().ReverseMap();
        }
    }
}
