using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerById;

internal class GetCombatPlayerByIdHandler(ICombatPlayerRepository repository, IMapper mapper) : IRequestHandler<GetCombatPlayerByIdQuery, CombatPlayerDto>
{
    private readonly ICombatPlayerRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CombatPlayerDto> Handle(GetCombatPlayerByIdQuery request, CancellationToken cancellationToken)
    {
        var combatLog = await _repository.GetByIdAsync(request.Id, cancellationToken);
        var map = _mapper.Map<CombatPlayerDto>(combatLog);

        return map;
    }
}
