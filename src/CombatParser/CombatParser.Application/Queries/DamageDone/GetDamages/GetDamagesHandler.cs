using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using CombatParser.Domain.Enums;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetDamages;

internal class GetDamagesHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> repository, IMapper mapper) : IRequestHandler<GetDamagesQuery, IEnumerable<DamageDoneDto>>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageDoneDto>> Handle(GetDamagesQuery request, CancellationToken cancellationToken)
    {
        var creatorTypes = new int[] { (int)CombatUnitType.PlayerCreature, (int)CombatUnitType.Player };
        var damages = await _repository.GetAsync(request.UnitId, request.Target, request.Creator, request.Spell, request.From, request.To, request.Page, request.PageSzie, cancellationToken, null, creatorTypes);
        var map = _mapper.Map<IEnumerable<DamageDoneDto>>(damages);

        return map;
    }
}

