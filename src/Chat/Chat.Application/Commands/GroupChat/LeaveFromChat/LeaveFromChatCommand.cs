using MediatR;

namespace Chat.Application.Commands.GroupChat.LeaveFromChat;

public record LeaveFromChatCommand(
    Guid Id,
    int GroupChatId
    ) : IRequest;
