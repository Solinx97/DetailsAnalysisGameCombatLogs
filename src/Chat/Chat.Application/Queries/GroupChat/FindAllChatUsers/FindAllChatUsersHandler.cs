using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Queries.GroupChat.FindAllChatUsers;

internal class FindAllChatUsersHandler(IGroupChatUserRepository repository, IMapper mapper) : IRequestHandler<FindAllChatUsersQuery, IEnumerable<GroupChatUserDto>>
{
    private readonly IGroupChatUserRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<GroupChatUserDto>> Handle(FindAllChatUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _repository.FindAllChatUsersAsync(request.ChatId, cancellationToken);
        var map = _mapper.Map<IEnumerable<GroupChatUserDto>>(users);

        return map;
    }
}
