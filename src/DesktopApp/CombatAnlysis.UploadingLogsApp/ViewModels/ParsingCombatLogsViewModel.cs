using AutoMapper;
using CombatAnalysis.UploadingLogsApp.Consts;
using CombatAnalysis.UploadingLogsApp.Core;
using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Interfaces;
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

public partial class ParsingCombatLogsViewModel : LocalizationViewModel
{
    private readonly IMapper _mapper;
    private readonly AppState _appState;
    private readonly IFileDialogService _fileDialogService;
    private readonly INavigationService _navigationService;
    private readonly WoW_5_5_4.CombatParser.Interfaces.ICombatParserService _wow_5_5_4_Parser;
    private readonly WoW_12_1_0.CombatParser.Interfaces.ICombatParserService _wow_12_1_0_Parser;
    private readonly ICombatParserAPIService _combatParserAPIService;

    private CancellationTokenSource _cts = new();

    private bool _processAborted;

    public ParsingCombatLogsViewModel()
    {
    }

    public ParsingCombatLogsViewModel(IMapper mapper, AppState appState, IFileDialogService fileDialogService, INavigationService navigationService,
        WoW_5_5_4.CombatParser.Interfaces.ICombatParserService wow_5_5_4_Parser, WoW_12_1_0.CombatParser.Interfaces.ICombatParserService wow_12_1_0_Parser, ICombatParserAPIService combatParserAPIService)
    {
        _mapper = mapper;
        _appState = appState;
        _fileDialogService = fileDialogService;
        _navigationService = navigationService;
        _wow_5_5_4_Parser = wow_5_5_4_Parser;
        _wow_12_1_0_Parser = wow_12_1_0_Parser;
        _combatParserAPIService = combatParserAPIService;

        CombatLogPaths.CollectionChanged += CombatLogPathsCollectionChanged;
    }

    public bool HasCombatLogs => CombatLogPaths.Count > 0;

    #region View model properties

    [ObservableProperty]
    public partial ObservableCollection<string> CombatLogNames { get; set; } = [];

    [ObservableProperty]
    public partial ObservableCollection<string> CombatLogPaths { get; set; } = [];

    [ObservableProperty]
    public partial bool IsParsing { get; set; }

    [ObservableProperty]
    public partial bool CombatLogUploadingFailed { get; set; }

    [ObservableProperty]
    public partial bool FileIsCorrect { get; set; } = true;

    [ObservableProperty]
    public partial bool IsAuth { get; set; }

    [ObservableProperty]
    private LogType logType;

    public bool IsPublic
    {
        get => LogType == LogType.Public;
        set
        {
            if (value)
                LogType = LogType.Public;
        }
    }

    public bool IsPrivate
    {
        get => LogType == LogType.Private;
        set
        {
            if (value)
                LogType = LogType.Private;
        }
    }

    [ObservableProperty]
    public partial LoadingStatus ResponseStatus { get; set; }

    [ObservableProperty]
    public partial int CombatsNumber { get; set; }

    [ObservableProperty]
    public partial bool ShowConnectMore { get; set; }

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

    [ObservableProperty]
    public partial CombatParserVersion ParserVersion { get; set; } = CombatParserVersion.WoWMidnight;

    #endregion

    #region Commands

    [RelayCommand]
    public async Task SelectFiles()
    {
        var files = await _fileDialogService.OpenFilesAsync();
        if (files.Length > 0)
        {
            CombatLogPaths.Clear();
        }

        foreach (var file in files)
        {
            CombatLogPaths.Add(file);
        }
    }

    [RelayCommand]
    public async Task SelectMoreFiles()
    {
        var files = await _fileDialogService.OpenFilesAsync();

        foreach (var file in files)
        {
            CombatLogPaths.Add(file);
        }
    }

    [RelayCommand]
    public void GetLogType(string logType)
    {
        var logTypeInt = int.Parse(logType);
        LogType = (LogType)logTypeInt;
    }

    [RelayCommand]
    public async Task OpenPlayerAnalysis()
    {
        CombatLogUploadingFailed = false;
        _appState.AllowLogout = false;

        await ProcessUploadingCombatLogsAsync(CombatLogPaths.ToList() ?? []);

        _appState.AllowLogout = true;

        _ = Task.Delay(TimeSpan.FromSeconds(20)).ContinueWith((task) => UploadingStatusShow = false);
    }

    [RelayCommand]
    public void CancelParsing()
    {
        _processAborted = true;
        _cts?.Cancel();

        IsParsing = false;
    }

    #endregion

    private static CancellationToken RequestCancelationToken()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        return token;
    }

    private void CombatLogPathsCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(HasCombatLogs));

        GetCombatLogNames();

        ShowConnectMore = CombatLogPaths.Count > 0;
    }

    private void GetCombatLogNames()
    {
        if (CombatLogPaths.Count == 0)
        {
            return;
        }

        CombatLogNames.Clear();

        foreach (var item in CombatLogPaths)
        {
            var split = item.Split(@"\");
            CombatLogNames.Add(split[^1]);
        }
    }

    private async Task ProcessUploadingCombatLogsAsync(List<string> combatLogPathes)
    {
        var tasks = combatLogPathes.Select(async combatLogPath =>
        {
            switch (CurrentCombatParserVersion.Version)
            {
                case CombatParserVersion.WoWMoPClassic:
                    return await _wow_5_5_4_Parser.FileCheckAsync(combatLogPath);
                case CombatParserVersion.WoWMidnight:
                    return await _wow_12_1_0_Parser.FileCheckAsync(combatLogPath);
                default:
                    return false;
            }
        });

        var result =  await Task.WhenAll(tasks);

        FileIsCorrect = result.All(x => x);
        if (result.Any(x => !x))
        {
            _ = Task.Delay(TimeSpan.FromSeconds(20)).ContinueWith((task) => FileIsCorrect = true);
            return;
        }

        IsParsing = true;

        var combats = await PrepareCombatDataAsync(combatLogPathes);
        if (combats.Count > 0)
        {
            await UploadingCombatLogAsync(combats);
        }

        IsParsing = false;
    }

    private async Task<List<CombatModel>> PrepareCombatDataAsync(List<string> combatLogPaths)
    {
        _cts = new CancellationTokenSource();

        WoW.CombatParser.Consts.API.CombatParserApi = API.CombatParserApi;
        var combats = await SelectParserVersionAsync(combatLogPaths);

        if (_processAborted)
        {
            _processAborted = false;
            return [];
        }

        return combats;
    }

    private async Task<List<CombatModel>> SelectParserVersionAsync(List<string> combatLogPaths)
    {
        var combats = new List<CombatModel>();
        switch (CurrentCombatParserVersion.Version)
        {
            case CombatParserVersion.WoWMoPClassic:
                await Task.Run(() => _wow_5_5_4_Parser.ParseAsync(combatLogPaths, _cts.Token));

                combats = _mapper.Map<List<CombatModel>>(_wow_5_5_4_Parser.Combats);
                await _combatParserAPIService.GetBossAsync(combats, false, _cts.Token);

                _wow_5_5_4_Parser.Clear();
                break;
            case CombatParserVersion.WoWMidnight:
                await Task.Run(() => _wow_12_1_0_Parser.ParseAsync(combatLogPaths, _cts.Token));

                combats = _mapper.Map<List<CombatModel>>(_wow_12_1_0_Parser.Combats);
                await _combatParserAPIService.GetBossAsync(combats, true, _cts.Token);

                _wow_12_1_0_Parser.Clear();
                break;
            default:
                break;
        }

        return combats;
    }

    private async Task UploadingCombatLogAsync(List<CombatModel> combats)
    {
        try
        {
            var createdCombatLogId = await _combatParserAPIService.SaveCombatLogAsync(combats, LogType, CancellationToken.None);

            UploadingStatusShow = true;

            await SaveCombatsAsync(createdCombatLogId, combats);
        }
        catch (Exception)
        {
            CombatLogUploadingFailed = true;
        }
    }

    private async Task SaveCombatsAsync(int combatLogId, List<CombatModel> combats)
    {
        try
        {
            UploadingInProgress = true;
            ResponseStatus = LoadingStatus.Pending;

            CurrentCombatNumber = 0;
            CombatsNumber = combats.Count(x => x.Boss.Id > 0);

            await _combatParserAPIService.SaveAsync(combats, combatLogId, CombatUploaded, RequestCancelationToken);

            ResponseStatus = LoadingStatus.Successful;
            UploadingInProgress = false;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            _appState.Logout();

            ResponseStatus = LoadingStatus.Failed;
            IsParsing = false;
            UploadingInProgress = false;

            await _navigationService.NavigateTo<LoginViewModel>();
        }
        catch (Exception)
        {
            ResponseStatus = LoadingStatus.Failed;
            IsParsing = false;
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
