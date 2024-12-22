using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using MvvmCross.ViewModels;

namespace CombatAnalysis.Core.Helpers;

internal class VMHandler<TBasicModel> : IVMHandler
    where TBasicModel : MvxViewModel
{
    public void PropertyUpdate<TViewModel>(IMvxViewModel context, string propertyName, object value)
        where TViewModel : MvxViewModel
    {
        typeof(TViewModel).GetProperty(propertyName)?.SetValue(context, value);
    }

    public void BasicPropertyUpdate(string propertyName, object value)
    {
        typeof(TBasicModel)?.GetProperty(propertyName)?.SetValue(BasicViewModel.Template, value);
    }
}