using AutoMapper;
using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerDAL.Entities;

namespace CombatAnalysis.CustomerBL.Mapping;

public class CustomerBLMapper : Profile
{
    public CustomerBLMapper()
    {
        CreateMap<AppUserDto, AppUser>().ReverseMap();;
        CreateMap<BannedUserDto, BannedUser>().ReverseMap();
        CreateMap<CustomerDto, Customer>().ReverseMap();
        CreateMap<FriendDto, Friend>().ReverseMap();
        CreateMap<RequestToConnectDto, RequestToConnect>().ReverseMap();
    }
}