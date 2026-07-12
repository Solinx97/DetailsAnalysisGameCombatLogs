using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetBossMap;

internal class GetBossMapHandler(IGenericRepository<BossMap, int> repository, IMapper mapper) : IRequestHandler<GetBossMapQuery, BossMapDto>
{
    private readonly IGenericRepository<BossMap, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<BossMapDto> Handle(GetBossMapQuery request, CancellationToken cancellationToken)
    {
        var bossMap = await _repository.GetByIdAsync(request.BossMapId, cancellationToken);
        var map = _mapper.Map<BossMapDto>(bossMap);

        return map;
    }
}
