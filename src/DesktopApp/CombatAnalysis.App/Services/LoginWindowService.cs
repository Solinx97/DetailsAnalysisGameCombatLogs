using CombatAnalysis.App.Windows;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.ViewModels.User;
using MvvmCross;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CombatAnalysis.App.Services;

internal class LoginWindowService : IAuthWindowService<AuthorizationViewModel>
{
    public async Task<bool> ShowAsync()
    {
        var vm = Mvx.IoCProvider.IoCConstruct<AuthorizationViewModel>();
        var window = new AuthorizationWindow
        {
            Owner = Application.Current.MainWindow,
            DataContext = vm
        };

        Action<bool>? handler = (result) =>
        {
            window.DialogResult = result;
            window.Close();
        };

        vm.LoginCompleted += handler;

        window.Closed += (sender, e) =>
        {
            vm.LoginCompleted -= handler;
        };

        var result = window.ShowDialog();

        return await Task.FromResult(result == true);
    }
}
