using AutoMapper;
using CombatParser.Application.DTOs.Dashboard;
using CombatParser.Domain.Data.Dashboard;
using MediatR;

namespace CombatParser.Application.Queries.Dashboards.GetDamageTaken;

internal class GetDamageTakenHandler(IDashboardRepository repository, IMapper mapper) : IRequestHandler<GetDamageTakenQuery, DashboardDto>
{
    private readonly IDashboardRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<DashboardDto> Handle(GetDamageTakenQuery request, CancellationToken cancellationToken)
    {
        var dashboard = await _repository.GetDamageTakenAsync(request.CombatLogId, request.CombatId, request.UnitName, request.ValueType, cancellationToken);
        var map = _mapper.Map<DashboardDto>(dashboard);

        return map;
    }
}
