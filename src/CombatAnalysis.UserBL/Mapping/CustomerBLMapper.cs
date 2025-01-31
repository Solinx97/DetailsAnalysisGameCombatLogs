using AutoMapper;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserDAL.Entities;

namespace CombatAnalysis.UserBL.Mapping;

public class CustomerBLMapper : Profile
{
    public CustomerBLMapper()
    {
        CreateMap<AppUserDto, AppUser>().ReverseMap(); ;
        CreateMap<BannedUserDto, BannedUser>().ReverseMap();
        CreateMap<CustomerDto, Customer>().ReverseMap();
        CreateMap<FriendDto, Friend>().ReverseMap();
        CreateMap<RequestToConnectDto, RequestToConnect>().ReverseMap();
    }
}