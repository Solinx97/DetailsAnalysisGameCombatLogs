using AutoMapper;
using CombatParser.Application.DTOs.CombatPlayerData;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Queries.GetCombatPlayerPositionById;

internal class GetCombatPlayerPositionByIdHandler(IGenericRepository<CombatPlayerPosition, string> repository, IMapper mapper) : IRequestHandler<GetCombatPlayerPositionByIdQuery, CombatPlayerPositionDto>
{
    private readonly IGenericRepository<CombatPlayerPosition, string> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CombatPlayerPositionDto> Handle(GetCombatPlayerPositionByIdQuery request, CancellationToken cancellationToken)
    {
        var position = await _repository.GetByIdAsync(request.Id, cancellationToken);
        var map = _mapper.Map<CombatPlayerPositionDto>(position);

        return map;
    }
}