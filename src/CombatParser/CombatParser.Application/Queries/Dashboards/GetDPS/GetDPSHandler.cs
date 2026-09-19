using AutoMapper;
using CombatParser.Application.DTOs.Dashboard;
using CombatParser.Domain.Data.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetDPS;

internal class GetDPSHandler(IDashboardRepository repository, IMapper mapper) : IRequestHandler<GetDPSQuery, DashboardDto>
{
    private readonly IDashboardRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<DashboardDto> Handle(GetDPSQuery request, CancellationToken cancellationToken)
    {
        var dashboard = await _repository.GetDamagePerSecondAsync(request.CombatLogId, request.CombatId, request.unitName, cancellationToken);
        var map = _mapper.Map<DashboardDto>(dashboard);

        return map;
    }
}