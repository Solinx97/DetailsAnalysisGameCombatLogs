using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using CombatParser.Domain.Enums;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.DamageTaken.GetDamageTakens;

internal class GetDamageTakensHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> repository, IMapper mapper) : IRequestHandler<GetDamageTakensQuery, IEnumerable<DamageDoneDto>>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageDoneDto>> Handle(GetDamageTakensQuery request, CancellationToken cancellationToken)
    {
        var creatorTypes = new int[] { (int)CombatUnitType.EnemyCreature, (int)CombatUnitType.Vehicle };
        var damageTakens = await _repository.GetAsync(request.CombatPlayerId, request.Target, request.Creator, request.Spell, request.From, request.To, request.Page, request.PageSzie, cancellationToken, null, creatorTypes);
        var map = _mapper.Map<IEnumerable<DamageDoneDto>>(damageTakens);

        return map;
    }
}

