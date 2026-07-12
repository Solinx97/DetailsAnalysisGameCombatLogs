using AutoMapper;
using CombatParser.Application.DTOs.Chart;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetCombatPlayerChart;

internal class GetCombatPlayerChartHandler(IChartRepository<Domain.Entities.CombatPlayerData.DamageTaken> repository, IMapper mapper) : IRequestHandler<GetCombatPlayerChartQuery, IEnumerable<ChartGenericDto>>
{
    private readonly IChartRepository<Domain.Entities.CombatPlayerData.DamageTaken> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ChartGenericDto>> Handle(GetCombatPlayerChartQuery request, CancellationToken cancellationToken)
    {
        var damages = await _repository.GetCombatPlayerChartAsync(request.CombatPlayerId, cancellationToken);
        var map = _mapper.Map<IEnumerable<ChartGenericDto>>(damages);

        return map;
    }
}
