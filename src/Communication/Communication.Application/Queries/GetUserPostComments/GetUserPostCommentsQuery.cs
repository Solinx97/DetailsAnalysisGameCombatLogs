using Communication.Application.DTOs.Post.General;
using MediatR;

namespace Communication.Application.Queries.GetUserPostComments;

public record GetUserPostCommentsQuery(
    int UserPostId,
    int Page,
    int PageSize
    ) : IRequest<AllUserPostCommentDto>;