using Communication.Application.DTOs.Community;
using MediatR;

namespace Communication.Application.Commands.CreateDiscussionComment;

public record CreateDiscussionCommentCommand(
    int CommunityDiscussionId,
    string Content,
    string AppUserId
    ) : IRequest<CommunityDiscussionCommentDto>;
