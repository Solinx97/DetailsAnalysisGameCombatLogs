using MediatR;

namespace Chat.Application.Queries.VoiceChat.IsChatExist;

public record IsChatExistQuery(
    int ChatId
    ) : IRequest<bool>;
