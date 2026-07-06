using AutoMapper;
using CombatParser.Application.DTOs.Chart;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.Resources.GetChart;

internal class GetChartHandler(IChartRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> repository, IMapper mapper) : IRequestHandler<GetChartQuery, IEnumerable<ChartGenericDto>>
{
    private readonly IChartRepository<Domain.Entities.CombatPlayerData.ResourceRecovery> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ChartGenericDto>> Handle(GetChartQuery request, CancellationToken cancellationToken)
    {
        var damages = await _repository.GetChartAsync(request.CombatPlayerId, cancellationToken);
        var map = _mapper.Map<IEnumerable<ChartGenericDto>>(damages);

        return map;
    }
}
