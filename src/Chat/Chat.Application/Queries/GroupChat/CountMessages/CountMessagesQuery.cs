using MediatR;

namespace Chat.Application.Queries.GroupChat.CountMessages;

public record CountMessagesQuery(
    int ChatId
    ) : IRequest<int>;
