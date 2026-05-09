using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetDamageTakensBySpell;

internal class GetDamageTakensBySpellHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> repository, IMapper mapper) : IRequestHandler<GetDamageTakensBySpellQuery, IEnumerable<DamageTakenDto>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageTakenDto>> Handle(GetDamageTakensBySpellQuery request, CancellationToken cancellationToken)
    {
        var damageTakens = await _repository.GetBySpellAsync(request.CombatPlayerId, request.Spell, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<DamageTakenDto>>(damageTakens);

        return map;
    }
}
