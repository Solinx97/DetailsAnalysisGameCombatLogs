using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Queries.GroupChat.FindChatUser;

internal class FindChatUserHandler(IGroupChatUserRepository repository, IMapper mapper) : IRequestHandler<FindChatUserQuery, GroupChatUserDto>
{
    private readonly IGroupChatUserRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<GroupChatUserDto> Handle(FindChatUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.FindChatUserAsync(request.AppUserId, request.ChatId, cancellationToken);
        var map = _mapper.Map<GroupChatUserDto>(user);

        return map;
    }
}
