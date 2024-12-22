using MvvmCross.ViewModels;

namespace CombatAnalysis.Core.Interfaces;

public interface IImprovedMvxViewModel : IMvxViewModel
{
    IVMHandler Handler { get; set; }

    IMvxViewModel Parent { get; set; }

    IMvxViewModel SavedViewModel { get; set; }
}