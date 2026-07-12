using AutoMapper;
using CombatParser.Application.DTOs.Chart;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageDone.GetGenericChart;

internal class GetGenericChartHandler(IChartRepository<Domain.Entities.CombatPlayerData.DamageDone> repository, IMapper mapper) : IRequestHandler<GetGenericChartQuery, Dictionary<string, ChartGenericDto[]>>
{
    private readonly IChartRepository<Domain.Entities.CombatPlayerData.DamageDone> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<Dictionary<string, ChartGenericDto[]>> Handle(GetGenericChartQuery request, CancellationToken cancellationToken)
    {
        var damages = await _repository.GetChartAsync(request.CombatId, cancellationToken);
        var map = _mapper.Map<Dictionary<string, ChartGenericDto[]>>(damages);

        return map;
    }
}