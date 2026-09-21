using AutoMapper;
using CombatParser.Application.DTOs.Dashboard;
using CombatParser.Domain.Data.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetDamageSpells;

internal class GetDamageSpellsHandler(IDashboardRepository repository, IMapper mapper) : IRequestHandler<GetDamageSpellsQuery, DashboardDto>
{
    private readonly IDashboardRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<DashboardDto> Handle(GetDamageSpellsQuery request, CancellationToken cancellationToken)
    {
        var dashboard = await _repository.GetDamageSpellsAsync(request.CombatLogId, request.BossName, request.CombatId, cancellationToken);
        var map = _mapper.Map<DashboardDto>(dashboard);

        return map;
    }
}