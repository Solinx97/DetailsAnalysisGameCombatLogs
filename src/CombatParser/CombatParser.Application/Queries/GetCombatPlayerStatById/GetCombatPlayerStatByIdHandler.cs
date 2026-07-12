using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerStatById;

internal class GetCombatPlayerStatByIdHandler(IGenericRepository<CombatPlayerStats, int> repository, IMapper mapper) : IRequestHandler<GetCombatPlayerStatByIdQuery, CombatPlayerStatsDto>
{
    private readonly IGenericRepository<CombatPlayerStats, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CombatPlayerStatsDto> Handle(GetCombatPlayerStatByIdQuery request, CancellationToken cancellationToken)
    {
        var stats = await _repository.GetByIdAsync(request.Id, cancellationToken);
        var map = _mapper.Map<CombatPlayerStatsDto>(stats);

        return map;
    }
}
