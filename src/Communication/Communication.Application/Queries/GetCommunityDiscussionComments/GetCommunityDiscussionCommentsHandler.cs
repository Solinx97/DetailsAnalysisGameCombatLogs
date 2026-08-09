using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunityDiscussionComments;

internal class GetCommunityDiscussionCommentsHandler(ICommunityDiscussionCommentRepository repository, IMapper mapper) : IRequestHandler<GetCommunityDiscussionCommentsQuery, IEnumerable<CommunityDiscussionCommentDto>>
{
    private readonly ICommunityDiscussionCommentRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CommunityDiscussionCommentDto>> Handle(GetCommunityDiscussionCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await _repository.GetByDiscussionIdAsync(request.DiscussionId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<CommunityDiscussionCommentDto>>(comments);

        return map;
    }
}
