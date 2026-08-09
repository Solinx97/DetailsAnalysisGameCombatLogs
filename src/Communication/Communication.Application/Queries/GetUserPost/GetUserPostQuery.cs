using Communication.Application.DTOs.Post;
using MediatR;

namespace Communication.Application.Queries.GetUserPost;

public record GetUserPostQuery(
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<UserPostDto>>;
