using MediatR;

namespace Chat.Application.Commands.GroupChat.CreateChat;

public record CreateChatCommand(
    string Name,
    string OwnerUsername,
    int InvitePeopleRule,
    int RemovePeopleRule,
    int PinMessageRule,
    int AnnouncementsRule,
    Guid OwnerId
    ) : IRequest;
