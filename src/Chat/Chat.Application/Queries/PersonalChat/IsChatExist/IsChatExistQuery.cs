using MediatR;

namespace Chat.Application.Queries.PersonalChat.IsChatExist;

public record IsChatExistQuery(
    Guid InitiatorId,
    Guid CompanionId
    ) : IRequest<bool>;
