using AutoMapper;
using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.UserApi.Models;

namespace CombatAnalysis.UserApi.Mapping;

public class UserApiMapper : Profile
{
    public UserApiMapper()
    {
        CreateMap<AppUserDto, AppUserModel>().ReverseMap();
        CreateMap<CustomerDto, CustomerModel>().ReverseMap();
        CreateMap<BannedUserDto, BannedUserModel>().ReverseMap();
        CreateMap<FriendDto, FriendModel>().ReverseMap();
        CreateMap<PostDto, PostModel>().ReverseMap();
        CreateMap<PostCommentDto, PostCommentModel>().ReverseMap();
        CreateMap<PostLikeDto, PostLikeModel>().ReverseMap();
        CreateMap<PostDislikeDto, PostDislikeModel>().ReverseMap();
        CreateMap<RequestToConnectDto, RequestToConnectModel>().ReverseMap();
    }
}
