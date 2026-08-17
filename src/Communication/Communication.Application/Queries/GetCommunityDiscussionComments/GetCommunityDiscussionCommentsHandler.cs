using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Application.DTOs.Community.General;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunityDiscussionComments;

internal class GetCommunityDiscussionCommentsHandler(ICommunityDiscussionCommentRepository repository, IMapper mapper) : IRequestHandler<GetCommunityDiscussionCommentsQuery, AllDiscussionCommentDto>
{
    private readonly ICommunityDiscussionCommentRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<AllDiscussionCommentDto> Handle(GetCommunityDiscussionCommentsQuery request, CancellationToken cancellationToken)
    {
        var (comments, count) = await _repository.GetByDiscussionIdAsync(request.DiscussionId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<CommunityDiscussionCommentDto>>(comments);

        return new AllDiscussionCommentDto(map, count);
    }
}
