using Communication.Application.DTOs.Post;
using MediatR;

namespace Communication.Application.Queries.GetUserPostByUserId;

public record GetUserPostByUserIdQuery(
    string AppUserId,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<UserPostDto>>;
