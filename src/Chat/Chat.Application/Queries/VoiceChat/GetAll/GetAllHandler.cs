using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;

namespace Chat.Application.Queries.VoiceChat.GetAll;

internal class GetAllHandler(IGenericRepository<Domain.Aggregates.VoiceChat, VoiceChatId> repository, IMapper mapper) : IRequestHandler<GetAllQuery, IEnumerable<VoiceChatDto>>
{
    private readonly IGenericRepository<Domain.Aggregates.VoiceChat, VoiceChatId> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<VoiceChatDto>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var chats = await _repository.GetAllAsync();
        var map = _mapper.Map<IEnumerable<VoiceChatDto>>(chats);

        return map;
    }
}
