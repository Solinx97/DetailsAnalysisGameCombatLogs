using MediatR;

namespace Chat.Application.Commands.GroupChat.UpdateChatRules;

public record UpdateChatRulesCommand(
    int ChatId,
    int InvitePeople,
    int RemovePeople,
    int PinMessage,
    int Announcements
    ) : IRequest;
