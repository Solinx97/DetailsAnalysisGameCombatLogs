using Communication.Application.DTOs.Community;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityDescussion;

public record CreateCommunityDescussionCommand(
    string Title,
    string Content,
    int CommunityId,
    Guid AppUserId
    ) : IRequest<CommunityDiscussionDto>;
