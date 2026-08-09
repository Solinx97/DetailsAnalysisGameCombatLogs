using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunityPostComments;

internal class GetCommunityPostCommentsHandler(ICommunityPostCommentRepository repository, IMapper mapper) : IRequestHandler<GetCommunityPostCommentsQuery, IEnumerable<CommunityPostCommentDto>>
{
    private readonly ICommunityPostCommentRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CommunityPostCommentDto>> Handle(GetCommunityPostCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await _repository.GetByCommunityPostIdAsync(request.CommunityPostId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<CommunityPostCommentDto>>(comments);

        return map;
    }
}
