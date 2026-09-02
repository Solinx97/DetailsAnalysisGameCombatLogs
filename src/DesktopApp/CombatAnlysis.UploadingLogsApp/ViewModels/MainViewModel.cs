using CombatAnalysis.Core.Core;
using CombatAnalysis.UploadingLogsApp.Core;
using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Interfaces;
using CombatAnalysis.UploadingLogsApp.Interfaces.Security;
using CombatAnalysis.UploadingLogsApp.Localizations;
using CombatAnalysis.UploadingLogsApp.Services;
using CombatAnalysis.UploadingLogsApp.ViewModels.Base;
using CombatAnalysis.UploadingLogsApp.ViewModels.User;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Caching.Memory;

namespace CombatAnalysis.UploadingLogsApp.ViewModels;

public partial class MainViewModel : LocalizationViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IMemoryCache _memoryCache;
    private readonly ISecurityStorage _securityStorage;

    public MainViewModel()
    {
    }

    public MainViewModel(NavigationStore navigationStore, AppState appState, INavigationService navigationService,
         IMemoryCache memoryCache, ISecurityStorage securityStorage)
    {
        NavigationStore = navigationStore;
        AppState = appState;
        AppState.AppName = AppInformation.Name;
        AppState.AppVersion = AppInformation.Version;

        _navigationService = navigationService;
        _memoryCache = memoryCache;
        _securityStorage = securityStorage;

        _navigationService.NavigateTo<LoginViewModel>();
    }

    public NavigationStore NavigationStore { get; }

    public AppState AppState { get; }

    [RelayCommand]
    public void Logout()
    {
        AppState.User = null;
        AppState.IsAuth = false;

        _memoryCache.Remove(nameof(MemoryCacheValue.User));
        _memoryCache.Remove(nameof(MemoryCacheValue.Customer));
        _memoryCache.Remove(nameof(MemoryCacheValue.AccessToken));
        _memoryCache.Remove(nameof(MemoryCacheValue.RefreshToken));

        _securityStorage.RemoveAccessToken();

        _navigationService.NavigateTo<LoginViewModel>();
    }

    [RelayCommand]
    private static void ChangeLanguage(string language)
    {
        LocalizationService.Instance.SetLanguage(language);
    }
}