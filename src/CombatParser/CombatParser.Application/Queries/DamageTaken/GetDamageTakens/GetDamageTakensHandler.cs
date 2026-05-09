using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetDamageTakens;

internal class GetDamageTakensHandler(ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.DamageTaken> repository, IMapper mapper) : IRequestHandler<GetDamageTakensQuery, IEnumerable<DamageTakenDto>>
{
    private readonly ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.DamageTaken> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageTakenDto>> Handle(GetDamageTakensQuery request, CancellationToken cancellationToken)
    {
        var damageTakens = await _repository.GetByCombatPlayerIdAsync(request.CombatPlayerId, request.Page, request.PageSzie, cancellationToken);
        var map = _mapper.Map<IEnumerable<DamageTakenDto>>(damageTakens);

        return map;
    }
}

