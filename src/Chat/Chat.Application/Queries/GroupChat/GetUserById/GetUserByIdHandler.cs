using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;

namespace Chat.Application.Queries.GroupChat.GetUserById;

internal class GetUserByIdHandler(IGenericRepository<GroupChatUser, GroupChatUserId> repository, IMapper mapper) : IRequestHandler<GetUserByIdQuery, GroupChatUserDto>
{
    private readonly IGenericRepository<GroupChatUser, GroupChatUserId> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<GroupChatUserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var messages = await _repository.GetByIdAsync(request.Id);
        var map = _mapper.Map<GroupChatUserDto>(messages);

        return map;
    }
}
