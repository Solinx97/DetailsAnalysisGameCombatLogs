using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Queries.GroupChat.CountMessages;

internal class CountMessagesHandler(IGroupChatMessageRepository repository) : IRequestHandler<CountMessagesQuery, int>
{
    private readonly IGroupChatMessageRepository _repository = repository;

    public async Task<int> Handle(CountMessagesQuery request, CancellationToken cancellationToken)
    {
        var count = await _repository.CountAsync(request.ChatId, cancellationToken);
        return count;
    }
}
