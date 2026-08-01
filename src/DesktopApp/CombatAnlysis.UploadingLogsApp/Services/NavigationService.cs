using CombatAnalysis.UploadingLogsApp.Interfaces;
using CombatAnalysis.UploadingLogsApp.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.Services;

internal partial class NavigationService(NavigationStore navigationStore, IServiceProvider serviceProvider) : ObservableObject, INavigationService
{
    private readonly NavigationStore _navigationStore = navigationStore;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task NavigateTo<TViewModel>()
        where TViewModel : ViewModelBase
    {
        var vm = _serviceProvider.GetRequiredService<TViewModel>();
        _navigationStore.CurrentViewModel = vm;

        if (vm is IAsyncInitializable vmInit)
        {
            await vmInit.InitializeAsync();
        }
    }
}
