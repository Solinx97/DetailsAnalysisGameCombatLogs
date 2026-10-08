using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;

namespace Chat.Application.Queries.GroupChat.GetRules;

internal class GetRulesHandler(IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> repository, IMapper mapper) : IRequestHandler<GetRulesQuery, GroupChatRulesDto?>
{
    private readonly IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<GroupChatRulesDto?> Handle(GetRulesQuery request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetByIdAsync(request.ChatId);
        var map = _mapper.Map<GroupChatRulesDto>(chat.Rules);

        return map;
    }
}
