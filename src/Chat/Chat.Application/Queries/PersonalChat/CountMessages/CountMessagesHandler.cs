using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Queries.PersonalChat.CountMessages;

internal class CountMessagesHandler(IPersonalChatMessageRepository repository) : IRequestHandler<CountMessagesQuery, int>
{
    private readonly IPersonalChatMessageRepository _repository = repository;

    public async Task<int> Handle(CountMessagesQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync(request.ChatId, cancellationToken);
        return count;
    }
}
