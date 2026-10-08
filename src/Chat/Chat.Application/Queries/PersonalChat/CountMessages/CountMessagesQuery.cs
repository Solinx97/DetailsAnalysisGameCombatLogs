using MediatR;

namespace Chat.Application.Queries.PersonalChat.CountMessages;

public record CountMessagesQuery(
    int ChatId
    ) : IRequest<int>;
