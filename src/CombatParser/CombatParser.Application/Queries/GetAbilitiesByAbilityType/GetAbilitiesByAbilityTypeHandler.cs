using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetAbilitiesByAbilityType;

internal class GetAbilitiesByAbilityTypeHandler(ICombatAbilityRepository repository, IMapper mapper) : IRequestHandler<GetAbilitiesByAbilityTypeQuery, IEnumerable<CombatAbilityDto>>
{
    private readonly ICombatAbilityRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CombatAbilityDto>> Handle(GetAbilitiesByAbilityTypeQuery request, CancellationToken cancellationToken)
    {
        var abilities = await _repository.GetByAbilityTypeAsync(request.CombatPlayerId, request.AbilityType, cancellationToken);
        var map = _mapper.Map<IEnumerable<CombatAbilityDto>>(abilities);

        return map;
    }
}