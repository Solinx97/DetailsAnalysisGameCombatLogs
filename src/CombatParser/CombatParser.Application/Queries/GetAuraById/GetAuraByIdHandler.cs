using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetAuraById;

internal class GetAuraByIdHandler(ICombatPlayerAuraRepository repository, IMapper mapper) : IRequestHandler<GetAuraByIdQuery, CombatPlayerAuraDto>
{
    private readonly ICombatPlayerAuraRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CombatPlayerAuraDto> Handle(GetAuraByIdQuery request, CancellationToken cancellationToken)
    {
        var allCombatLogs = await _repository.GetByIdAsync(request.Id, cancellationToken);
        var map = _mapper.Map<CombatPlayerAuraDto>(allCombatLogs);

        return map;
    }
}
