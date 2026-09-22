using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using CombatParser.Domain.Enums;
using MediatR;

namespace CombatParser.Application.Queries.GetUniqueUnitNames;

internal class GetUniqueUnitNamesHandler(IUnitRepository repository, IMapper mapper) : IRequestHandler<GetUniqueUnitNamesQuery, IEnumerable<UniqueUnitNameDto>>
{
    private readonly IUnitRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<UniqueUnitNameDto>> Handle(GetUniqueUnitNamesQuery request, CancellationToken cancellationToken)
    {
        var types = new int[] { (int)CombatUnitType.Player, (int)CombatUnitType.Pet, (int)CombatUnitType.Vehicle, (int)CombatUnitType.EnemyCreature };
        var uniqueUnitsName = await _repository.GetUniqueNamesAsync(request.CombatLogId, request.BossName, types, cancellationToken);
        var map = _mapper.Map<IEnumerable<UniqueUnitNameDto>>(uniqueUnitsName);

        return map;
    }
}
