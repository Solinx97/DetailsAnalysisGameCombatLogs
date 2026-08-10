using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunityDiscussionById;

internal class GetCommunityDiscussionByIdHandler(IGenericRepository<CommunityDiscussion, int> repository, IMapper mapper) : IRequestHandler<GetCommunityDiscussionByIdQuery, CommunityDiscussionDto>
{
    private readonly IGenericRepository<CommunityDiscussion, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityDiscussionDto> Handle(GetCommunityDiscussionByIdQuery request, CancellationToken cancellationToken)
    {
        var discussion = await _repository.GetByIdAsync(request.Id, cancellationToken);
        var map = _mapper.Map<CommunityDiscussionDto>(discussion);

        return map;
    }
}
