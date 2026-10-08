using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;

namespace Chat.Application.Queries.GroupChat.GetById;

internal class GetByIdHandler(IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> repository, IMapper mapper) : IRequestHandler<GetByIdQuery, GroupChatDto>
{
    private readonly IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<GroupChatDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetByIdAsync(request.Id);
        var map = _mapper.Map<GroupChatDto>(chat);

        return map;
    }
}
