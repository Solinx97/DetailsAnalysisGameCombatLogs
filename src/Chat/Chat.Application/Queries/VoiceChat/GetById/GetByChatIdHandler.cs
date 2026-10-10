using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Queries.VoiceChat.GetById;

internal class GetByChatIdHandler(IVoiceChatRepository repository, IMapper mapper) : IRequestHandler<GetByChatIdQuery, VoiceChatDto>
{
    private readonly IVoiceChatRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<VoiceChatDto> Handle(GetByChatIdQuery request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetByChatIdAsync(request.ChatId, cancellationToken);
        var map = _mapper.Map<VoiceChatDto>(chat);

        return map;
    }
}
