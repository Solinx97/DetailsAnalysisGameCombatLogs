using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Queries.GroupChat.GetMessagesByChatId;

internal class GetMessagesByChatIdHandler(IGroupChatMessageRepository repository, IMapper mapper) : IRequestHandler<GetMessagesByChatIdQuery, IEnumerable<GroupChatMessageDto>>
{
    private readonly IGroupChatMessageRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<GroupChatMessageDto>> Handle(GetMessagesByChatIdQuery request, CancellationToken cancellationToken)
    {
        var messages = await _repository.GetByChatIdAsync(request.ChatId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<GroupChatMessageDto>>(messages);

        return map;
    }
}
