using AutoMapper;
using CombatParser.Application.DTOs.Dashboard;
using CombatParser.Domain.Data.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetHeal;

internal class GetHealHandler(IDashboardRepository repository, IMapper mapper) : IRequestHandler<GetHealQuery, DashboardDto>
{
    private readonly IDashboardRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<DashboardDto> Handle(GetHealQuery request, CancellationToken cancellationToken)
    {
        var dashboard = await _repository.GetHealAsync(request.CombatLogId, request.BossName, request.CombatId, request.CreatorName, request.TargetName, request.ValueType, cancellationToken);
        var map = _mapper.Map<DashboardDto>(dashboard);

        return map;
    }
}
