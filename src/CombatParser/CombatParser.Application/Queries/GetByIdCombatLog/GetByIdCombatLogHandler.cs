using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetByIdCombatLog;

internal class GetByIdCombatLogHandler(IGenericRepository<CombatLog, int> repository, IMapper mapper) : IRequestHandler<GetByIdCombatLogQuery, CombatLogDto>
{
    private readonly IGenericRepository<CombatLog, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CombatLogDto> Handle(GetByIdCombatLogQuery request, CancellationToken cancellationToken)
    {
        var combatLog = await _repository.GetByIdAsync(request.Id, cancellationToken);
        var map = _mapper.Map<CombatLogDto>(combatLog);

        return map;
    }
}