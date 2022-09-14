using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.ChatApi.Models;

namespace CombatAnalysis.ChatApi.Mapping
{
    public class ChatMapper : Profile
    {
        public ChatMapper()
        {
            CreateMap<PersonalChatDto, PersonalChatModel>().ReverseMap();
            CreateMap<MessageDataDto, MessageDataModel>().ReverseMap();
        }
    }
}
