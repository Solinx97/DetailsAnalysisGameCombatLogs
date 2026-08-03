using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatLogsByLogType;

internal class GetCombatLogsByLogTypeHandler(ICombatLogRepository repository, IMapper mapper) : IRequestHandler<GetCombatLogsByLogTypeQuery, IEnumerable<CombatLogDto>>
{
    private readonly ICombatLogRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CombatLogDto>> Handle(GetCombatLogsByLogTypeQuery request, CancellationToken cancellationToken)
    {
        var combatLogs = await _repository.GetByLogTypeAsync(request.LogType, request.AppUserId, cancellationToken);
        var map = _mapper.Map<IEnumerable<CombatLogDto>>(combatLogs);

        return map;
    }
}