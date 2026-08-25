using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunityById;

internal class GetCommunityByIdHandler(IGenericRepository<Community, int> repository, IMapper mapper) : IRequestHandler<GetCommunityByIdQuery, CommunityDto>
{
    private readonly IGenericRepository<Community, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityDto> Handle(GetCommunityByIdQuery request, CancellationToken cancellationToken)
    {
        var communities = await _repository.GetByIdAsync(request.Id, cancellationToken);
        var map = _mapper.Map<CommunityDto>(communities);

        return map;
    }
}