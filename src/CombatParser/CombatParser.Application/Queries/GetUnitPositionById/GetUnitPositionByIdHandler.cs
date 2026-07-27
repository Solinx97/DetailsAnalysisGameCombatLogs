using AutoMapper;
using CombatParser.Application.DTOs;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using MediatR;

namespace CombatParser.Application.Queries.GetUnitPositionById;

internal class GetUnitPositionByIdHandler(IGenericRepository<UnitPosition, string> repository, IMapper mapper) : IRequestHandler<GetUnitPositionByIdQuery, UnitPositionDto>
{
    private readonly IGenericRepository<UnitPosition, string> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<UnitPositionDto> Handle(GetUnitPositionByIdQuery request, CancellationToken cancellationToken)
    {
        var position = await _repository.GetByIdAsync(request.Id, cancellationToken);
        var map = _mapper.Map<UnitPositionDto>(position);

        return map;
    }
}