using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Application.DTOs.Post.General;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetUserPostComments;

internal class GetUserPostCommentsHandler(IUserPostCommentRepository repository, IMapper mapper) : IRequestHandler<GetUserPostCommentsQuery, AllUserPostCommentDto>
{
    private readonly IUserPostCommentRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<AllUserPostCommentDto> Handle(GetUserPostCommentsQuery request, CancellationToken cancellationToken)
    {
        var (comments, count) = await _repository.GetByUserPostIdAsync(request.UserPostId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<UserPostCommentDto>>(comments);

        return new AllUserPostCommentDto(map, count);
    }
}
