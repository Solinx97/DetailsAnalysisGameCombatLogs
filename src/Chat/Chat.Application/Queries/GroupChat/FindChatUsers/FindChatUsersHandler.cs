using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Queries.GroupChat.FindChatUsers;

internal class FindChatUsersHandler(IGroupChatUserRepository repository, IMapper mapper) : IRequestHandler<FindChatUsersQuery, IEnumerable<GroupChatUserDto>>
{
    private readonly IGroupChatUserRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<GroupChatUserDto>> Handle(FindChatUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _repository.FindChatUsersAsync(request.AppUserId, cancellationToken);
        var map = _mapper.Map<IEnumerable<GroupChatUserDto>>(users);

        return map;
    }
}
