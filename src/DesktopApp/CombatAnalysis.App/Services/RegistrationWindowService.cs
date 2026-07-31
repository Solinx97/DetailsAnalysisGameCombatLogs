using CombatAnalysis.App.Windows;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.ViewModels.User;
using MvvmCross;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CombatAnalysis.App.Services;

internal class RegistrationWindowService : IAuthWindowService<RegistrationViewModel>
{
    public async Task<bool> ShowAsync()
    {
        var vm = Mvx.IoCProvider.IoCConstruct<RegistrationViewModel>();
        var window = new RegistrationWindow
        {
            Owner = Application.Current.MainWindow,
            DataContext = vm
        };

        Action<bool>? handler = (result) =>
        {
            window.DialogResult = result;
            window.Close();
        };

        vm.RegistrationCompleted += handler;

        window.Closed += (sender, e) =>
        {
            vm.RegistrationCompleted -= handler;
        };

        var result = window.ShowDialog();

        return await Task.FromResult(result == true);
    }
}
