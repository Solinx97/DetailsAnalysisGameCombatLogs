using CombatAnalysis.Core.Interfaces;
using MvvmCross.ViewModels;

namespace CombatAnalysis.Core.ViewModels.Base;

public abstract class ParentTemplate : MvxViewModel, IImprovedMvxViewModel
{
    private IImprovedMvxViewModel _basicTemplate;

    public ParentTemplate()
    {
        BasicTemplate = Basic;
    }

    public static IImprovedMvxViewModel Basic { get; set; }

    public IVMHandler Handler { get; set; }

    public IMvxViewModel Parent { get; set; }

    public IMvxViewModel SavedViewModel { get; set; }

    public IImprovedMvxViewModel BasicTemplate
    {
        get { return _basicTemplate; }
        set
        {
            SetProperty(ref _basicTemplate, value);
        }
    }
}

public abstract class ParentTemplate<T> : MvxViewModel<T>, IImprovedMvxViewModel
    where T : notnull
{
    private IImprovedMvxViewModel _basicTemplate;

    public ParentTemplate()
    {
        BasicTemplate = ParentTemplate.Basic;
    }

    public IVMHandler Handler { get; set; }

    public IMvxViewModel Parent { get; set; }

    public IMvxViewModel SavedViewModel { get; set; }

    public IImprovedMvxViewModel BasicTemplate
    {
        get { return _basicTemplate; }
        set
        {
            SetProperty(ref _basicTemplate, value);
        }
    }

    public override void Prepare(T parameter)
    {
        ChildPrepare(parameter);
    }

    protected abstract void ChildPrepare(T parameter);
}
