using AutoMapper;
using CombatParser.Application.DTOs.Dashboard;
using CombatParser.Domain.Data.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetDashboard;

internal class GetDashboardHandler(IDashboardRepository repository, IMapper mapper) : IRequestHandler<GetDashboardQuery, DashboardDto[]>
{
    private readonly IDashboardRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<DashboardDto[]> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        var dashboards = await _repository.GetAsync(request.CombatLogId, cancellationToken);
        var map = _mapper.Map<DashboardDto[]>(dashboards);

        return map;
    }
}