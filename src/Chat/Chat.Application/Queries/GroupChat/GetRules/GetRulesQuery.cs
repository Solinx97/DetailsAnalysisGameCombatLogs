using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Queries.GroupChat.GetRules;

public record GetRulesQuery(
    int ChatId
    ) : IRequest<GroupChatRulesDto?>;
