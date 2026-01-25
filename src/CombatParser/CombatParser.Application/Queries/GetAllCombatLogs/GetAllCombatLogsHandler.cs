using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetAllCombatLogs;

internal class GetAllCombatLogsHandler(IGenericRepository<CombatLog, int> repository, IMapper mapper) : IRequestHandler<GetAllCombatLogsQuery, IEnumerable<CombatLogDto>>
{
    private readonly IGenericRepository<CombatLog, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CombatLogDto>> Handle(GetAllCombatLogsQuery request, CancellationToken cancellationToken)
    {
        var allCombatLogs = await _repository.GetAllAsync(cancellationToken);
        var map = _mapper.Map<IEnumerable<CombatLogDto>>(allCombatLogs);

        return map;
    }
}
