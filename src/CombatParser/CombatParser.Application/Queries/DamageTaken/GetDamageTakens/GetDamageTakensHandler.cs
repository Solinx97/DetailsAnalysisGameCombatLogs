using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetDamageTakens;

internal class GetDamageTakensHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> repository, IMapper mapper) : IRequestHandler<GetDamageTakensQuery, IEnumerable<DamageTakenDto>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageTakenDto>> Handle(GetDamageTakensQuery request, CancellationToken cancellationToken)
    {
        var damageTakens = await _repository.GetAsync(request.CombatPlayerId, request.Target, request.Creator, request.Spell, request.From, request.To, request.Page, request.PageSzie, cancellationToken);
        var map = _mapper.Map<IEnumerable<DamageTakenDto>>(damageTakens);

        return map;
    }
}

