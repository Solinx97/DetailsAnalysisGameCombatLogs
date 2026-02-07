using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data.Filters;
using MediatR;

namespace CombatParser.Application.Queries.DamageTaken.GetDamageTakensByCreator;

internal class GetDamageTakensByCreatorHandler(IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> repository, IMapper mapper) : IRequestHandler<GetDamageTakensByCreatorQuery, IEnumerable<DamageTakenDto>>
{
    private readonly IGeneralFilterRepository<Domain.Entities.CombatPlayerData.DamageTaken> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DamageTakenDto>> Handle(GetDamageTakensByCreatorQuery request, CancellationToken cancellationToken)
    {
        var heals = await _repository.GetByTargetAsync(request.CombatPlayerId, request.Target, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<DamageTakenDto>>(heals);

        return map;
    }
}
