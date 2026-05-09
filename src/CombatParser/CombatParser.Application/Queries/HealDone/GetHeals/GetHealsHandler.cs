using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetHeals;

internal class GetHealsHandler(ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.HealDone> repository, IMapper mapper) : IRequestHandler<GetHealsQuery, IEnumerable<HealDoneDto>>
{
    private readonly ICombatPlayerDataRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<HealDoneDto>> Handle(GetHealsQuery request, CancellationToken cancellationToken)
    {
        var heals = await _repository.GetByCombatPlayerIdAsync(request.CombatPlayerId, request.Page, request.PageSzie, cancellationToken);
        var map = _mapper.Map<IEnumerable<HealDoneDto>>(heals);

        return map;
    }
}

