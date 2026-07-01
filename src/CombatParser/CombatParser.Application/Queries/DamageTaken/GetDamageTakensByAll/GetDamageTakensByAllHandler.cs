using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetDamageTakensByAll;

internal class GetDamageTakensByAllHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> repository, IMapper mapper) : IRequestHandler<GetDamageTakensByAllQuery, IEnumerable<DamageTakenDto>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageTakenDto>> Handle(GetDamageTakensByAllQuery request, CancellationToken cancellationToken)
    {
        var damageTakens = await _repository.GetByAllCreatorsAsync(request.CombatPlayerId, request.Creator, request.Spell, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<DamageTakenDto>>(damageTakens);

        return map;
    }
}