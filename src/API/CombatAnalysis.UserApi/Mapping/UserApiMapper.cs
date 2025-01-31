using AutoMapper;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserApi.Models;

namespace CombatAnalysis.UserApi.Mapping;

internal class UserApiMapper : Profile
{
    public UserApiMapper()
    {
        CreateMap<AppUserDto, AppUserModel>().ReverseMap();
        CreateMap<CustomerDto, CustomerModel>().ReverseMap();
        CreateMap<BannedUserDto, BannedUserModel>().ReverseMap();
        CreateMap<FriendDto, FriendModel>().ReverseMap();
        CreateMap<RequestToConnectDto, RequestToConnectModel>().ReverseMap();
    }
}
