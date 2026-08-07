using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunity;

internal class GetCommunityHandler(IGenericRepository<Community, int> repository, IMapper mapper) : IRequestHandler<GetCommunityQuery, IEnumerable<CommunityDto>>
{
    private readonly IGenericRepository<Community, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CommunityDto>> Handle(GetCommunityQuery request, CancellationToken cancellationToken)
    {
        var communities = await _repository.GetAsync(request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<CommunityDto>>(communities);

        return map;
    }
}