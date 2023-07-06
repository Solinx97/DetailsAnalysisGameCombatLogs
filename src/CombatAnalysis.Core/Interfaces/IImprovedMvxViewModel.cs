using MvvmCross.ViewModels;

namespace CombatAnalysis.Core.Interfaces;

public interface IImprovedMvxViewModel
{
    IVMHandler Handler { get; set; }

    IMvxViewModel Parent { get; set; }

    IMvxViewModel SavedViewModel { get; set; }
}