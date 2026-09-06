using AutoMapper;
using CombatParser.Application.Interfaces;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.WoWMoPClassic;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerStatById;

internal class GetCombatPlayerStatByIdHandler(IGenericRepository<WoWMoPClassicPlayerStats, int> repository, IMapper mapper) : IRequestHandler<GetCombatPlayerStatByIdQuery, IPlayerStatsDto>
{
    private readonly IGenericRepository<WoWMoPClassicPlayerStats, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IPlayerStatsDto> Handle(GetCombatPlayerStatByIdQuery request, CancellationToken cancellationToken)
    {
        var stats = await _repository.GetByIdAsync(request.Id, cancellationToken);
        var map = _mapper.Map<IPlayerStatsDto>(stats);

        return map;
    }
}
