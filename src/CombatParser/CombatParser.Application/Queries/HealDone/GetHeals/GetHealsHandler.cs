using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.HealDone.GetHeals;

internal class GetHealsHandler(IGeneralRepository<Domain.Entities.CombatPlayerData.HealDone> repository, IMapper mapper) : IRequestHandler<GetHealsQuery, IEnumerable<HealDoneDto>>
{
    private readonly IGeneralRepository<Domain.Entities.CombatPlayerData.HealDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<HealDoneDto>> Handle(GetHealsQuery request, CancellationToken cancellationToken)
    {
        var heals = await _repository.GetAsync(request.CombatPlayerId, request.Target, request.Creator, request.Spell, request.From, request.To, request.Page, request.PageSzie, cancellationToken);
        var map = _mapper.Map<IEnumerable<HealDoneDto>>(heals);

        return map;
    }
}

