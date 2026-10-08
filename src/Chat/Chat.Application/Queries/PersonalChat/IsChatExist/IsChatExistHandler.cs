using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Queries.PersonalChat.IsChatExist;

internal class IsChatExistHandler(IPersonalChatRepository repository) : IRequestHandler<IsChatExistQuery, bool>
{
    private readonly IPersonalChatRepository _repository = repository;

    public async Task<bool> Handle(IsChatExistQuery request, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsExistAsync(request.InitiatorId, request.CompanionId, cancellationToken);

        return isExist;
    }
}
