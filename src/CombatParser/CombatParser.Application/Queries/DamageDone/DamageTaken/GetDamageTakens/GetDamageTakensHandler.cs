using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.DamageTaken.GetDamageTakens;

internal class GetDamageTakensHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> repository, IMapper mapper) : IRequestHandler<GetDamageTakensQuery, IEnumerable<DamageDoneDto>>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageDoneDto>> Handle(GetDamageTakensQuery request, CancellationToken cancellationToken)
    {
        var targetsHash = new string[] { "0x514", "0x512", "0x511" };
        var damageTakens = await _repository.GetDamageAsync(request.CombatPlayerId, request.Target, request.Creator, request.Spell, request.From, request.To, request.Page, request.PageSzie, targetsHash, cancellationToken);
        var map = _mapper.Map<IEnumerable<DamageDoneDto>>(damageTakens);

        return map;
    }
}

