using MediatR;

namespace Communication.Application.Queries.CountCommunityPostComment;

public record CountCommunityPostCommentQuery(
    int CommunityPostId
    ) : IRequest<int>;
