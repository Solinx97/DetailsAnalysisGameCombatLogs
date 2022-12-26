using MvvmCross.ViewModels;

namespace CombatAnalysis.Core.Interfaces;

public interface IImprovedMvxViewModel
{
    IViewModelConnect Handler { get; set; }

    IMvxViewModel Parent { get; set; }
}