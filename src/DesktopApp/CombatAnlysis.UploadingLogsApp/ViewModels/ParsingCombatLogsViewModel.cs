using AutoMapper;
using CombatAnalysis.UploadingLogsApp.Consts;
using CombatAnalysis.UploadingLogsApp.Core;
using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Interfaces;
using CombatAnalysis.UploadingLogsApp.Models;
using CombatAnalysis.UploadingLogsApp.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.ViewModels;

public partial class ParsingCombatLogsViewModel : LocalizationViewModel
{
    private readonly IMapper _mapper;
    private readonly AppState _appState;
    private readonly IFileDialogService _fileDialogService;
    private readonly WoW_5_5_4.CombatParser.Interfaces.ICombatParserService _wow_5_5_4_Parser;
    private readonly WoW_12_1_0.CombatParser.Interfaces.ICombatParserService _wow_12_1_0_Parser;
    private readonly ICombatParserAPIService _combatParserAPIService;

    private CancellationTokenSource _cts = new();

    private bool _processAborted;

    public ParsingCombatLogsViewModel()
    {
    }

    public ParsingCombatLogsViewModel(IMapper mapper, AppState appState, IFileDialogService fileDialogService,
        WoW_5_5_4.CombatParser.Interfaces.ICombatParserService wow_5_5_4_Parser, WoW_12_1_0.CombatParser.Interfaces.ICombatParserService wow_12_1_0_Parser, ICombatParserAPIService combatParserAPIService)
    {
        _mapper = mapper;
        _appState = appState;
        _fileDialogService = fileDialogService;
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
    public partial CombatParserVersion ParserVersion { get; set; } = CombatParserVersion.WoWMidnight;

    #endregion

    #region Commands

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

    private async Task CombatLogFileValidateAsync(List<string> combatLogPaths)
    {
        foreach (var item in combatLogPaths)
        {
            FileIsCorrect = await _wow_5_5_4_Parser.FileCheckAsync(item);
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

        WoW.CombatParser.Consts.API.CombatParserApi = API.CombatParserApi;
        var combats = await SelectParserVersion(combatLogPaths);

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

    private async Task<List<CombatModel>> SelectParserVersion(List<string> combatLogPaths)
    {
        var combats = new List<CombatModel>();
        switch (CurrentCombatParserVersion.Version)
        {
            case CombatParserVersion.WoWMoPClassic:
                await _wow_5_5_4_Parser.ParseAsync(combatLogPaths, _cts.Token);

                combats = _mapper.Map<List<CombatModel>>(_wow_5_5_4_Parser.Combats);
                await _combatParserAPIService.GetBossAsync(combats, false, _cts.Token);

                _wow_5_5_4_Parser.Clear();
                break;
            case CombatParserVersion.WoWMidnight:
                await _wow_12_1_0_Parser.ParseAsync(combatLogPaths, _cts.Token);

                combats = _mapper.Map<List<CombatModel>>(_wow_12_1_0_Parser.Combats);
                await _combatParserAPIService.GetBossAsync(combats, true, _cts.Token);

                _wow_12_1_0_Parser.Clear();
                break;
            default:
                break;
        }

        combats = [.. combats.Select(x =>
        {
            x.GameVersion = (int)CurrentCombatParserVersion.Version;
            return x;
        })];

        return combats;
    }
}
