using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunitiesByUserId;

internal class GetCommunitiesByUserIdHandler(ICommunityRepository repository, IMapper mapper) : IRequestHandler<GetCommunitiesByUserIdQuery, IEnumerable<CommunityDto>>
{
    private readonly ICommunityRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CommunityDto>> Handle(GetCommunitiesByUserIdQuery request, CancellationToken cancellationToken)
    {
        var communities = await _repository.GetByUserIdAsync(request.AppUserId, cancellationToken);
        var map = _mapper.Map<IEnumerable<CommunityDto>>(communities);

        return map;
    }
}
