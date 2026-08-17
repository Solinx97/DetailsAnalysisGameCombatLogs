using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Application.DTOs.Community.General;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunityDiscussions;

internal class GetCommunityDiscussionsHandler(ICommunityDiscussionRepository repository, IMapper mapper) : IRequestHandler<GetCommunityDiscussionsQuery, AllDiscussionDto>
{
    private readonly ICommunityDiscussionRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<AllDiscussionDto> Handle(GetCommunityDiscussionsQuery request, CancellationToken cancellationToken)
    {
        var (discussions, count) = await _repository.GetAsync(request.CommunityId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<CommunityDiscussionDto>>(discussions);

        return new AllDiscussionDto(map, count);
    }
}