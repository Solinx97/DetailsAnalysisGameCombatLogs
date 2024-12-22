using MvvmCross.ViewModels;

namespace CombatAnalysis.Core.Interfaces;

public interface IVMHandler
{
    void PropertyUpdate<TViewModel>(IMvxViewModel context, string propertyName, object value) where TViewModel : MvxViewModel;

    void BasicPropertyUpdate(string propertyName, object value);
}