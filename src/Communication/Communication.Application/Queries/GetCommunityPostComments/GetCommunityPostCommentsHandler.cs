using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Application.DTOs.Post.General;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunityPostComments;

internal class GetCommunityPostCommentsHandler(ICommunityPostCommentRepository repository, IMapper mapper) : IRequestHandler<GetCommunityPostCommentsQuery, AllCommunityPostCommentsDto>
{
    private readonly ICommunityPostCommentRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<AllCommunityPostCommentsDto> Handle(GetCommunityPostCommentsQuery request, CancellationToken cancellationToken)
    {
        var (comments, count) = await _repository.GetByCommunityPostIdAsync(request.CommunityPostId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<CommunityPostCommentDto>>(comments);

        return new AllCommunityPostCommentsDto(map, count);
    }
}
