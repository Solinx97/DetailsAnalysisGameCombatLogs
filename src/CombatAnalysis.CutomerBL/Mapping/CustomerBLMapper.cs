using AutoMapper;
using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.DAL.Entities.User;

namespace CombatAnalysis.CustomerBL.Mapping;

public class CustomerBLMapper : Profile
{
    public CustomerBLMapper()
    {
        CreateMap<AppUserDto, AppUser>().ReverseMap();;
        CreateMap<BannedUserDto, BannedUser>().ReverseMap();
        CreateMap<CustomerDto, Customer>().ReverseMap();
        CreateMap<FriendDto, Friend>().ReverseMap();
        CreateMap<PostLikeDto, PostLike>().ReverseMap();
        CreateMap<PostDislikeDto, PostDislike>().ReverseMap();
        CreateMap<PostDto, Post>().ReverseMap();
        CreateMap<RequestToConnectDto, RequestToConnect>().ReverseMap();
    }
}