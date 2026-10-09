using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Queries.GroupChat.FindAllChatUsers;

public record FindAllChatUsersQuery(
    int ChatId
    ) : IRequest<IEnumerable<GroupChatUserDto>>;
