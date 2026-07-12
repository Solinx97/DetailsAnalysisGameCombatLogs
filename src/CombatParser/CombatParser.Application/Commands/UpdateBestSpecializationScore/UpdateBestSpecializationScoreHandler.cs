using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using MediatR;

namespace CombatParser.Application.Commands.UpdateBestSpecializationScore;

internal class UpdateBestSpecializationScoreHandler(IGenericRepository<BestSpecializationScore, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateBestSpecializationScoreQuery>
{
    private readonly IGenericRepository<BestSpecializationScore, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateBestSpecializationScoreQuery request, CancellationToken cancellationToken)
    {
        var specializationScore = await _repository.GetByIdAsync(request.Id, cancellationToken);

        specializationScore.Update(request.DamageDone, request.HealDone);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
