using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Consts;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.CombatPlayer.GetPlayerHealthesBeforeDied;

internal class GetPlayerHealthesBeforeDiedHandler(IUnitRepository repository, IMapper mapper) : IRequestHandler<GetPlayerHealthesBeforeDiedQuery, IEnumerable<UnitHealthDto>>
{
    private readonly IUnitRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<UnitHealthDto>> Handle(GetPlayerHealthesBeforeDiedQuery request, CancellationToken cancellationToken)
    {
        var toTome = TimeSpan.Parse(request.To);
        var from = toTome - PlayerDeathValue.IntervalBeforeDied;

        var unitsHealth = await _repository.GetUnitsHealthByIntervalAsync(request.UnitId, from.ToString(), request.To, cancellationToken);
        var map = _mapper.Map<IEnumerable<UnitHealthDto>>(unitsHealth);

        return map;
    }
}

