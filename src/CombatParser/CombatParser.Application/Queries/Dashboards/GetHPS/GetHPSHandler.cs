using AutoMapper;
using CombatParser.Application.DTOs.Dashboard;
using CombatParser.Domain.Data.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetHPS;

internal class GetHPSHandler(IDashboardRepository repository, IMapper mapper) : IRequestHandler<GetHPSQuery, DashboardDto>
{
    private readonly IDashboardRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<DashboardDto> Handle(GetHPSQuery request, CancellationToken cancellationToken)
    {
        var dashboard = await _repository.GetHealPerSecondAsync(request.CombatLogId, request.CombatId, request.UnitName, cancellationToken);
        var map = _mapper.Map<DashboardDto>(dashboard);

        return map;
    }
}
