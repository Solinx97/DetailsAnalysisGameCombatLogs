using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Queries.GetByIdCombat;

internal class GetByIdCombatHandler(ICombatRepository repository, IMapper mapper) : IRequestHandler<GetByIdCombatQuery, CombatDto>
{
    private readonly ICombatRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CombatDto> Handle(GetByIdCombatQuery request, CancellationToken cancellationToken)
    {
        var combat = await _repository.GetByIdAsync(request.Id, cancellationToken);
        var map = _mapper.Map<CombatDto>(combat);

        return map;
    }
}
