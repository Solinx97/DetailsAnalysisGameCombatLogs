using AutoMapper;
using CombatParser.Application.DTOs.Dashboard;
using CombatParser.Domain.Data.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetHealSpells;

internal class GetHealSpellsHandler(IDashboardRepository repository, IMapper mapper) : IRequestHandler<GetHealSpellsQuery, DashboardDto>
{
    private readonly IDashboardRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<DashboardDto> Handle(GetHealSpellsQuery request, CancellationToken cancellationToken)
    {
        var dashboard = await _repository.GetHealSpellsAsync(request.CombatLogId, request.CombatId, request.UnitName, cancellationToken);
        var map = _mapper.Map<DashboardDto>(dashboard);

        return map;
    }
}