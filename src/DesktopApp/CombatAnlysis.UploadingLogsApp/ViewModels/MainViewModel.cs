using CombatAnalysis.UploadingLogsApp.Interfaces;
using CombatAnalysis.UploadingLogsApp.Services;
using CombatAnalysis.UploadingLogsApp.ViewModels.User;

namespace CombatAnalysis.UploadingLogsApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;

    public MainViewModel()
    {
    }

    public NavigationStore NavigationStore { get; }

    public MainViewModel(NavigationStore navigationStore, INavigationService navigationService)
    {
        NavigationStore = navigationStore;
        _navigationService = navigationService;

        _navigationService.NavigateTo<LoginViewModel>();
    }
}