using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.DTO.Community;
using CombatAnalysis.BL.DTO.Post;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Entities.Community;
using CombatAnalysis.DAL.Entities.Post;

namespace CombatAnalysis.BL.Mapping;

public class BLMapper : Profile
{
    public BLMapper()
    {
        CreateMap<PersonalChatDto, PersonalChat>().ReverseMap();
        CreateMap<PersonalChatMessageDto, PersonalChatMessage>().ReverseMap();
        CreateMap<InviteToGroupChatDto, InviteToGroupChat>().ReverseMap();
        CreateMap<GroupChatDto, GroupChat>().ReverseMap();
        CreateMap<GroupChatMessageDto, GroupChatMessage>().ReverseMap();
        CreateMap<GroupChatUserDto, GroupChatUser>().ReverseMap();
        CreateMap<CombatLogDto, CombatLog>().ReverseMap();
        CreateMap<CombatLogByUserDto, CombatLogByUser>().ReverseMap();
        CreateMap<CombatDto, Combat>().ReverseMap();
        CreateMap<CombatPlayerDto, CombatPlayer>().ReverseMap();
        CreateMap<DamageDoneDto, DamageDone>().ReverseMap();
        CreateMap<DamageDoneGeneralDto, DamageDoneGeneral>().ReverseMap();
        CreateMap<HealDoneDto, HealDone>().ReverseMap();
        CreateMap<HealDoneGeneralDto, HealDoneGeneral>().ReverseMap();
        CreateMap<DamageTakenDto, DamageTaken>().ReverseMap();
        CreateMap<DamageTakenGeneralDto, DamageTakenGeneral>().ReverseMap();
        CreateMap<ResourceRecoveryDto, ResourceRecovery>().ReverseMap();
        CreateMap<ResourceRecoveryGeneralDto, ResourceRecoveryGeneral>().ReverseMap();
        CreateMap<CommunityDto, Community>().ReverseMap();
        CreateMap<CommunityUserDto, CommunityUser>().ReverseMap();
        CreateMap<InviteToCommunityDto, InviteToCommunity>().ReverseMap();
        CreateMap<PostDto, Post>().ReverseMap();
        CreateMap<PostLikeDto, PostLike>().ReverseMap();
        CreateMap<PostDislikeDto, PostDislike>().ReverseMap();
        CreateMap<PostCommentDto, PostComment>().ReverseMap();
        CreateMap<CommunityPostDto, CommunityPost>().ReverseMap();
        CreateMap<UserPostDto, UserPost>().ReverseMap();
    }
}