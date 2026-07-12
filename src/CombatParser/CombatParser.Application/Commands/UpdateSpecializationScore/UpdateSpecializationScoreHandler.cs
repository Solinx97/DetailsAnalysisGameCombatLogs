using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using MediatR;

namespace CombatParser.Application.Commands.UpdateSpecializationScore;

internal class UpdateSpecializationScoreHandler(IGenericRepository<SpecializationScore, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateSpecializationScoreCommand>
{
    private readonly IGenericRepository<SpecializationScore, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateSpecializationScoreCommand request, CancellationToken cancellationToken)
    {
        var specializationScore = await _repository.GetByIdAsync(request.Id, cancellationToken);

        specializationScore.Update(request.DamageScore, request.HealScore);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}