using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Queries.PersonalChat.GetMessagesByChatId;

internal class GetMessagesByChatIdHandler(IPersonalChatMessageRepository repository, IMapper mapper) : IRequestHandler<GetMessagesByChatIdQuery, IEnumerable<PersonalChatMessageDto>>
{
    private readonly IPersonalChatMessageRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<PersonalChatMessageDto>> Handle(GetMessagesByChatIdQuery request, CancellationToken cancellationToken)
    {
        var messages = await _repository.GetByChatIdAsync(request.ChatId, request.Page,request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<PersonalChatMessageDto>>(messages);

        return map;
    }
}
