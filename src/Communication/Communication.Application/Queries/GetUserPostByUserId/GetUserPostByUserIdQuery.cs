using Communication.Application.DTOs.Post.General;
using MediatR;

namespace Communication.Application.Queries.GetUserPostByUserId;

public record GetUserPostByUserIdQuery(
    Guid AppUserId,
    int Page,
    int PageSize
    ) : IRequest<AllUserPostsDto>;
