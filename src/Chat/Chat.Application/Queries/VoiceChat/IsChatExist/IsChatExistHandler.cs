using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Queries.VoiceChat.IsChatExist;

internal class IsChatExistHandler(IVoiceChatRepository repository) : IRequestHandler<IsChatExistQuery, bool>
{
    private readonly IVoiceChatRepository _repository = repository;

    public async Task<bool> Handle(IsChatExistQuery request, CancellationToken cancellationToken)
    {
        var isExist = await _repository.IsChatExistAsync(request.ChatId, cancellationToken);

        return isExist;
    }
}
