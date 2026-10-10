using AutoMapper;
using Chat.Application.DTOs;
using CombatAnalysis.ChatAPI.Models;
using CombatAnalysis.ChatAPI.Patches;

namespace CombatAnalysis.ChatAPI.Mapping;

internal class ChatMapper : Profile
{
    public ChatMapper()
    {
        CreateMap<PersonalChatDto, PersonalChatModel>().ReverseMap();
        CreateMap<PersonalChatMessageDto, PersonalChatMessageModel>().ReverseMap();
        CreateMap<GroupChatDto, GroupChatModel>().ReverseMap();
        CreateMap<GroupChatDto, GroupChatPatch>().ReverseMap();
        CreateMap<GroupChatRulesDto, GroupChatRulesModel>().ReverseMap();
        CreateMap<GroupChatMessageDto, GroupChatMessageModel>().ReverseMap();
        CreateMap<GroupChatUserDto, GroupChatUserModel>().ReverseMap();

        CreateMap<PersonalChatPatch, PersonalChatDto>()
         .ConstructUsing(dto => new PersonalChatDto {
             InitiatorUnreadMessages = dto.InitiatorUnreadMessages,
             CompanionUnreadMessages = dto.CompanionUnreadMessages
         }).ReverseMap();
    }
}
