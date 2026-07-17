using MvvmCross.ViewModels;

namespace CombatAnalysis.Core.Interfaces;

public interface IAuthWindowService<TWindow>
    where TWindow : class, IMvxViewModel
{
    Task<bool> ShowAsync();
}
