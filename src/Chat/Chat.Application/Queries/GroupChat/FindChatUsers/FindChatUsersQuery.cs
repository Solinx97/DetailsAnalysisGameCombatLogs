using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Queries.GroupChat.FindChatUsers;

public record FindChatUsersQuery(
    Guid AppUserId
    ) : IRequest<IEnumerable<GroupChatUserDto>>;
