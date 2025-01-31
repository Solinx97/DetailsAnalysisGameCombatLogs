using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using MvvmCross.ViewModels;

namespace CombatAnalysis.Core.ViewModels.Base;

public abstract class ParentTemplate : MvxViewModel
{
    public IImprovedMvxViewModel Basic { get; set; } = BasicViewModel.Template ?? throw new InvalidOperationException($"{nameof(BasicViewModel.Template)} is null");
}

public abstract class ParentTemplate<TModel> : MvxViewModel<TModel>
    where TModel : notnull
{
    public IImprovedMvxViewModel Basic { get; set; } = BasicViewModel.Template ?? throw new InvalidOperationException($"{nameof(BasicViewModel.Template)} is null");
}