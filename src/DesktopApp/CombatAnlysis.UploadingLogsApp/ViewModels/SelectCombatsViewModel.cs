using CombatAnalysis.UploadingLogsApp.Core;
using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Interfaces;
using CombatAnalysis.UploadingLogsApp.Interfaces.Data;
using CombatAnalysis.UploadingLogsApp.Models;
using CombatAnalysis.UploadingLogsApp.ViewModels.Base;
using CombatAnalysis.UploadingLogsApp.ViewModels.User;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.ViewModels;

public partial class SelectCombatsViewModel(INavigationService navigationService, ICombatService combatService, ICombatParserAPIService combatParserAPIService, AppState appState)
    : LocalizationViewModel, IAsyncInitializable
{
    private readonly INavigationService _navigationService = navigationService;
    private readonly ICombatService _combatService = combatService;
    private readonly ICombatParserAPIService _combatParserAPIService = combatParserAPIService;
    private readonly AppState _appState = appState;

    #region View model properties

    [ObservableProperty]
    public partial bool ShowOnlySupported { get; set; } = true;

    [ObservableProperty]
    public partial ObservableCollection<CreateCombatModel> Combats { get; set; }

    [ObservableProperty]
    public partial bool CombatLogUploadingFailed { get; set; }

    [ObservableProperty]
    public partial LoadingStatus ResponseStatus { get; set; }

    [ObservableProperty]
    public partial int CombatsNumber { get; set; }

    [ObservableProperty]
    public partial bool UploadingInProgress { get; set; }

    [ObservableProperty]
    public partial bool UploadingStatusShow { get; set; }

    [ObservableProperty]
    public partial string DungeonName { get; set; }

    [ObservableProperty]
    public partial string Name { get; set; }

    [ObservableProperty]
    public partial int CurrentCombatNumber { get; set; }

    [ObservableProperty]
    public partial string UploadingInformation { get; set; }

    #endregion

    [RelayCommand]
    public async Task UploadSelectedCombats()
    {
        await UploadingCombatLogAsync([.. Combats]);

        _combatService.Clear();

        _ = Task.Delay(TimeSpan.FromSeconds(20)).ContinueWith((task) => UploadingStatusShow = false);
    }

    [RelayCommand]
    public async Task BackToParsing()
    {
        await _navigationService.NavigateTo<ParsingCombatLogsViewModel>();
    }

    [RelayCommand]
    public void SetShowOnlySupported(bool isOnlySupported)
    {
        Combats = new ObservableCollection<CreateCombatModel>(combatService.GetCombats(isOnlySupported));
    }

    [RelayCommand]
    public async Task SetCheckAll()
    {
        Combats = [.. Combats.Select(x => {
                x.IsSelected = true;
                return x;
            })];
    }

    [RelayCommand]
    public async Task SetUncheckAll()
    {
        Combats = [.. Combats.Select(x => {
                x.IsSelected = false;
                return x;
            })];
    }

    public async Task InitializeAsync()
    {
        Combats = new ObservableCollection<CreateCombatModel>(combatService.GetCombats(true));
    }

    private static CancellationToken RequestCancelationToken()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        return token;
    }

    private async Task UploadingCombatLogAsync(List<CreateCombatModel> combats)
    {
        try
        {
            UploadingInProgress = true;

            var createdCombatLogId = await _combatParserAPIService.SaveCombatLogAsync(combats, _combatService.LogType, CancellationToken.None);

            UploadingStatusShow = true;

            await SaveCombatsAsync(createdCombatLogId, combats);
        }
        catch (Exception)
        {
            CombatLogUploadingFailed = true;
        }
    }

    private async Task SaveCombatsAsync(int combatLogId, List<CreateCombatModel> combats)
    {
        try
        {
            ResponseStatus = LoadingStatus.Pending;

            CurrentCombatNumber = 0;
            CombatsNumber = combats.Count(x => x.IsSupported && x.IsSelected);

            await _combatParserAPIService.SaveAsync(combats, combatLogId, CombatUploaded, RequestCancelationToken);

            ResponseStatus = LoadingStatus.Successful;
            UploadingInProgress = false;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _appState.Logout();

            ResponseStatus = LoadingStatus.Failed;
            UploadingInProgress = false;

            await _navigationService.NavigateTo<LoginViewModel>();
        }
        catch (Exception)
        {
            ResponseStatus = LoadingStatus.Failed;
            UploadingInProgress = false;
        }
    }

    private void CombatUploaded(string dungeonName, string name, string uploadingInfomration)
    {
        DungeonName = dungeonName;
        Name = name;
        UploadingInformation = uploadingInfomration;

        CurrentCombatNumber++;
    }
}
