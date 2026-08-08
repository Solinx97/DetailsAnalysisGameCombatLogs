using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunityDiscussions;

internal class GetCommunityDiscussionsHandler(ICommunityDiscussionRepository repository, IMapper mapper) : IRequestHandler<GetCommunityDiscussionsQuery, IEnumerable<CommunityDiscussionDto>>
{
    private readonly ICommunityDiscussionRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CommunityDiscussionDto>> Handle(GetCommunityDiscussionsQuery request, CancellationToken cancellationToken)
    {
        var communityDiscussions = await _repository.GetAsync(request.CommunityId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<CommunityDiscussionDto>>(communityDiscussions);

        return map;
    }
}

