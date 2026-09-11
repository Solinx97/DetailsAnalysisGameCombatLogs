using CombatAnalysis.Core.Core;
using CombatAnalysis.UploadingLogsApp.Core;
using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Interfaces;
using CombatAnalysis.UploadingLogsApp.Localizations;
using CombatAnalysis.UploadingLogsApp.Services;
using CombatAnalysis.UploadingLogsApp.ViewModels.Base;
using CombatAnalysis.UploadingLogsApp.ViewModels.User;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace CombatAnalysis.UploadingLogsApp.ViewModels;

public partial class MainViewModel : LocalizationViewModel
{
    private readonly INavigationService _navigationService;

    public MainViewModel()
    {
    }

    public MainViewModel(NavigationStore navigationStore, AppState appState, INavigationService navigationService)
    {
        NavigationStore = navigationStore;
        AppState = appState;
        AppState.AppName = AppInformation.Name;
        AppState.AppVersion = AppInformation.Version;

        _navigationService = navigationService;

        _navigationService.NavigateTo<LoginViewModel>();
    }

    public NavigationStore NavigationStore { get; }

    public AppState AppState { get; }

    #region View model properties

    [ObservableProperty]
    public partial CombatParserVersion ParserVersion { get; set; } = CombatParserVersion.WoWMidnight;

    #endregion

    [RelayCommand]
    private void Logout()
    {
        AppState.Logout();

        _navigationService.NavigateTo<LoginViewModel>();
    }

    [RelayCommand]
    private void SwitchParserVersion(string version)
    {
        ParserVersion = Enum.Parse<CombatParserVersion>(version);
        CurrentCombatParserVersion.Version = ParserVersion;
    }

    [RelayCommand]
    private static void ChangeLanguage(string language)
    {
        LocalizationService.Instance.SetLanguage(language);
    }
}