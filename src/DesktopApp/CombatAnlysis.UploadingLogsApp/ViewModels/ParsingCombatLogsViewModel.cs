using AutoMapper;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.UploadingLogsApp.Consts;
using CombatAnalysis.UploadingLogsApp.Core;
using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Interfaces;
using CombatAnalysis.UploadingLogsApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.ViewModels;

public partial class ParsingCombatLogsViewModel : ViewModelBase
{
    private readonly IMapper _mapper;
    private readonly AppState _appState;
    private readonly IFileDialogService _fileDialogService;
    private readonly ICombatParserService _parser;
    private readonly ICombatParserAPIService _combatParserAPIService;

    private CancellationTokenSource _cts = new();

    private bool _processAborted;

    public ParsingCombatLogsViewModel()
    {
    }

    public ParsingCombatLogsViewModel(IMapper mapper, AppState appState, IFileDialogService fileDialogService,
        ICombatParserService parser, ICombatParserAPIService combatParserAPIService)
    {
        _mapper = mapper;
        _appState = appState;
        _fileDialogService = fileDialogService;
        _parser = parser;
        _combatParserAPIService = combatParserAPIService;

        CombatLogPaths.CollectionChanged += CombatLogPathsCollectionChanged;
    }

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

    #endregion

    public bool HasCombatLogs => CombatLogPaths.Count > 0;

    [RelayCommand]
    public async Task SelectFiles()
    {
        var files = await _fileDialogService.OpenFilesAsync();

        CombatLogPaths.Clear();

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

        await CombatLogFileValidateAsync(CombatLogPaths.ToList() ?? []);

        _appState.AllowLogout = true;

        _ = Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith((task) => UploadingStatusShow = false);
    }

    [RelayCommand]
    public void CancelParsing()
    {
        _processAborted = true;
        _cts?.Cancel();

        IsParsing = false;
    }

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

    private async Task CombatLogFileValidateAsync(List<string> combatLogPaths)
    {
        foreach (var item in combatLogPaths)
        {
            FileIsCorrect = await _parser.FileCheckAsync(item);
            if (!FileIsCorrect) return;
        }

        IsParsing = true;

        var combats = await PrepareCombatDataAsync(combatLogPaths);
        if (combats.Count > 0)
        {
            await UploadingCombatLogAsync(combats);
        }

        IsParsing = false;
    }

    private async Task<List<CombatModel>> PrepareCombatDataAsync(List<string> combatLogPaths)
    {
        _cts = new CancellationTokenSource();

        CombatParser.Consts.API.CombatParserApi = API.CombatParserApi;
        await _parser.ParseAsync(combatLogPaths, _cts.Token);

        var combats = _mapper.Map<List<CombatModel>>(_parser.Combats);
        await _combatParserAPIService.GetBossAsync(combats, _cts.Token);

        AppStaticData.PreparedCombatsCount = _parser.Combats.Count;

        _parser.Clear();

        if (_processAborted)
        {
            _processAborted = false;
            return [];
        }

        return combats;
    }

    private async Task UploadingCombatLogAsync(List<CombatModel> combats)
    {
        var createdCombatLog = await _combatParserAPIService.SaveCombatLogAsync(combats, LogType, CancellationToken.None);
        if (createdCombatLog.AppUserId == null)
        {
            CombatLogUploadingFailed = true;

            return;
        }

        UploadingStatusShow = true;

        await SaveCombatsAsync(createdCombatLog, combats);
    }

    private async Task SaveCombatsAsync(CombatLogModel combatLog, List<CombatModel> combats)
    {
        try
        {
            UploadingInProgress = true;
            ResponseStatus = LoadingStatus.Pending;

            CurrentCombatNumber = 0;
            CombatsNumber = combats.Count;

            await _combatParserAPIService.SaveAsync(combats, combatLog, CombatUploaded, RequestCancelationToken);

            ResponseStatus = LoadingStatus.Successful;
            UploadingInProgress = false;
        }
        catch (Exception)
        {
            ResponseStatus = LoadingStatus.Failed;
            UploadingInProgress = false;
        }
    }

    private void CombatUploaded(string dungeonName, string name)
    {
        DungeonName = dungeonName;
        Name = name;

        CurrentCombatNumber++;
    }
}
