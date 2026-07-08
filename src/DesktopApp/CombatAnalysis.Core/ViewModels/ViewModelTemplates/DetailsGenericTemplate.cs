using AutoMapper;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Entities;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.ViewModels.Base;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Resources;

namespace CombatAnalysis.Core.ViewModels.ViewModelTemplates;

public abstract class DetailsGenericTemplate<DetailsModel, GeneralDetailsModel> : ParentTemplate<CombatPlayerModel>
    where DetailsModel : class, IDetailsEntity
    where GeneralDetailsModel : class, IDetailsEntity
{
    protected readonly IHttpClientHelper _httpClient;
    protected readonly ILogger _logger;
    protected readonly IMapper _mapper;
    protected readonly ICacheService? _cacheService;
    protected readonly ICombatParserAPIService _combatParserAPIService;

    protected List<GeneralDetailsModel>? _allGeneralInformations;
    protected List<DetailsModel>? _allDetailsInformations;

    protected readonly int _pageSize = 20;

    private CombatPlayerModel? _parameter;
    private int _page = 1;
    private int _count;
    private int _maxPages;
    private string? _apiName;
    private string? _generalApiName;

    private bool _isShowFilters;
    private string? _selectedDamageDoneSource;
    private string? _selectedPlayer;
    private int _selectedPlayerId;
    private long _totalValue;
    private ObservableCollection<DetailsModel>? _detailsInformations;
    private ObservableCollection<GeneralDetailsModel>? _generalInformations;
    private ObservableCollection<string>? _sources;
    private int _detailsTypeSelectedIndex;
    private CancellationTokenSource _cancelToken;
    private LoadingStatus _loadingStatus;

    public DetailsGenericTemplate(IHttpClientHelper httpClient, ILogger logger, IMapper mapper,
        ICombatParserAPIService combatParserAPIService)
    {
        _httpClient = httpClient;
        _logger = logger;
        _mapper = mapper;
        _combatParserAPIService = combatParserAPIService;

        FirstPageCommand = new MvxAsyncCommand(LoadFirstPageAsync);
        PrevPageCommand = new MvxAsyncCommand(LoadPrevPageAsync);
        NextPageCommand = new MvxAsyncCommand(LoadNextPageAsync);
        LastPageCommand = new MvxAsyncCommand(LoadLastPageAsync);

        _httpClient.BaseAddress = API.CombatParserApi;
    }

    public DetailsGenericTemplate(IHttpClientHelper httpClient, ILogger logger, IMapper mapper,
        ICacheService cacheService, ICombatParserAPIService combatParserAPIService) : this(httpClient, logger, mapper, combatParserAPIService)
    {
        _cacheService = cacheService;
    }

    #region Commands

    public IMvxAsyncCommand FirstPageCommand { get; private set; }

    public IMvxAsyncCommand PrevPageCommand { get; private set; }

    public IMvxAsyncCommand NextPageCommand { get; private set; }

    public IMvxAsyncCommand LastPageCommand { get; private set; }

    #endregion

    #region Properties

    public static CombatModel? SelectedCombat { get; set; }

    public static bool IsNeedSave { get; set; }

    public int Page
    {
        get { return _page; }
        set
        {
            SetProperty(ref _page, value);
        }
    }

    public int MaxPages
    {
        get { return _maxPages; }
        set
        {
            SetProperty(ref _maxPages, value);
        }
    }

    public int Count
    {
        get { return _count; }
        set
        {
            SetProperty(ref _count, value);
        }
    }

    public string? SelectedSource
    {
        get { return _selectedDamageDoneSource; }
        set
        {
            SetProperty(ref _selectedDamageDoneSource, value);
            _ = LoadDetailsAsync(Page, _pageSize);
        }
    }

    public string? SelectedPlayer
    {
        get { return _selectedPlayer; }
        set
        {
            SetProperty(ref _selectedPlayer, value);
        }
    }

    public int SelectedPlayerId
    {
        get { return _selectedPlayerId; }
        set
        {
            SetProperty(ref _selectedPlayerId, value);
        }
    }

    public long TotalValue
    {
        get { return _totalValue; }
        set
        {
            SetProperty(ref _totalValue, value);
        }
    }

    public bool IsShowFilters
    {
        get { return _isShowFilters; }
        set
        {
            SetProperty(ref _isShowFilters, value);
        }
    }

    public int DetailsTypeSelectedIndex
    {
        get { return _detailsTypeSelectedIndex; }
        set
        {
            SetProperty(ref _detailsTypeSelectedIndex, value);

            if (value == 0)
            {
                IsShowFilters = false;
            }
        }
    }

    public ObservableCollection<DetailsModel>? DetailsInformations
    {
        get { return _detailsInformations; }
        set
        {
            SetProperty(ref _detailsInformations, value);
        }
    }

    public ObservableCollection<GeneralDetailsModel>? GeneralInformations
    {
        get { return _generalInformations; }
        set
        {
            SetProperty(ref _generalInformations, value);
        }
    }

    public ObservableCollection<string>? Sources
    {
        get { return _sources; }
        set
        {
            SetProperty(ref _sources, value);
        }
    }

    public LoadingStatus LoadingStatus
    {
        get { return _loadingStatus; }
        set
        {
            SetProperty(ref _loadingStatus, value);
        }
    }

    #endregion

    public override void Prepare(CombatPlayerModel parameter)
    {
        _parameter = parameter;

        SelectedPlayer = parameter.Player.Username;
        SelectedPlayerId = parameter.Id;
        TotalValue = parameter.DamageDone;
    }

    public override async Task Initialize()
    {
        if (_parameter == null)
        {
            return;
        }

        if (SelectedCombat?.Id > 0)
        {
            GetAPINameFromDetailsModelName();
            GetAPINameFromGeneralDetailsModelName();
        }

        _cancelToken = new CancellationTokenSource();

        LoadingStatus = LoadingStatus.Pending;

        await LoadGeneralDetailsAsync();
        await LoadDetailsAsync(Page, _pageSize);
        await LoadCountAsync();

        GetSources();

        LoadingStatus = LoadingStatus.Successful;

        await base.Initialize();
    }

    public override void ViewDestroy(bool viewFinishing = true)
    {
        _cancelToken?.Cancel();

        base.ViewDestroy(viewFinishing);
    }

    public void GetSources()
    {
        var sources = DetailsInformations?.Select(x => x.Spell).Distinct().ToList();
        if (sources == null)
        {
            return;
        }

        var resourceMangaer = new ResourceManager("CombatAnalysis.App.Localizations.Resources.DetailsGeneralTemplate.Resource", Assembly.Load("CombatAnalysis.App"));
        var allSourcesName = resourceMangaer.GetString(SourcesType.All.ToString());
        if (!string.IsNullOrEmpty(allSourcesName))
        {
            sources.Insert(0, allSourcesName);
        }

        Sources = new ObservableCollection<string>(sources);
    }

    protected abstract void GetDetailsFromCache(CombatPlayerModel? parameter);

    protected void LoadDetailsFromCache(int page, int pageSize)
    {
        GetDetailsFromCache(_parameter);

        if (_allDetailsInformations != null && _allDetailsInformations.Count > 0)
        {
            var range = _allDetailsInformations.GetRange((page - 1) * pageSize, pageSize > _allDetailsInformations.Count ? _allDetailsInformations.Count - 1 : pageSize);
            DetailsInformations = new(range ?? []);
        }
    }

    protected void LoadGeneralDetailsFromCache()
    {
        GetDetailsFromCache(_parameter);

        GeneralInformations = new(_allGeneralInformations ?? []);
    }

    private async Task LoadFirstPageAsync()
    {
        Page = 1;
        await LoadDetailsAsync(Page, _pageSize);
    }

    private async Task LoadNextPageAsync()
    {
        if (Page != MaxPages)
        {
            Page++;
            await LoadDetailsAsync(Page, _pageSize);
        }
    }

    private async Task LoadPrevPageAsync()
    {
        if (Page > 1)
        {
            Page--;
            await LoadDetailsAsync(Page, _pageSize);
        }
    }

    private async Task LoadLastPageAsync()
    {
        Page = MaxPages;
        await LoadDetailsAsync(Page, _pageSize);
    }

    private async Task LoadCountAsync()
    {
        var count = await _combatParserAPIService.LoadCountAsync($"{_apiName}/count/{SelectedPlayerId}", _cancelToken.Token);
        Count = count;

        if (count == 0)
        {
            CalculateMaxPages(_allDetailsInformations != null ? _allDetailsInformations.Count : 1);
        }
        else
        {
            CalculateMaxPages(Count);
        }
    }

    private async Task LoadGeneralDetailsAsync()
    {
        var generalInformations = await _combatParserAPIService.LoadCombatDetailsAsync<GeneralDetailsModel>(_httpClient, _logger, $"{_generalApiName}/getByCombatPlayerId/{SelectedPlayerId}", _cancelToken.Token);
        if (generalInformations.Any())
        {
            _allGeneralInformations = [.. generalInformations.ToList()];
            GeneralInformations = new ObservableCollection<GeneralDetailsModel>(_allGeneralInformations);
        }
        else
        {
            LoadGeneralDetailsFromCache();
        }
    }

    private async Task LoadDetailsAsync(int page, int pageSize)
    {
        var source = SelectedSource;
        if (string.IsNullOrEmpty(source))
        {
            source = "NONE";
        }
        else if (string.Equals(SelectedSource, SourcesType.All.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            LoadDetailsFromCache(page, pageSize);
            return;
        }

        var detailsInformations = await _combatParserAPIService.LoadCombatDetailsAsync<DetailsModel>(_httpClient, _logger, $"{_apiName}/getAll?combatPlayerId={SelectedPlayerId}&target=NONE&creator=NONE&spell={source}&from=00:00:00&to=00:00:00&page={page}&pageSize={pageSize}", _cancelToken.Token);
        if (detailsInformations.Any())
        {
            _allDetailsInformations = [.. detailsInformations.ToList()];
            DetailsInformations = new ObservableCollection<DetailsModel>(_allDetailsInformations);
        }
        else
        {
            LoadDetailsFromCache(page, pageSize);
        }
    }

    private void Filter()
    {
        if (_allDetailsInformations == null || string.IsNullOrEmpty(SelectedSource))
        {
            return;
        }

        if (string.Equals(SelectedSource, SourcesType.All.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            LoadDetailsFromCache(Page, _pageSize);

            return;
        }

        DetailsInformations = _allDetailsInformations.Any(x => x.Spell == SelectedSource)
            ? new ObservableCollection<DetailsModel>(_allDetailsInformations.Where(x => x.Spell == SelectedSource))
            : new ObservableCollection<DetailsModel>(_allDetailsInformations);

        var range = DetailsInformations.ToList().GetRange((Page - 1) * _pageSize, _pageSize);
        DetailsInformations = new(range ?? []);

        var count = DetailsInformations.Count();
        CalculateMaxPages(count);
    }

    private void GetAPINameFromDetailsModelName()
    {
        _apiName = typeof(DetailsModel).Name.Replace("Model", "");
    }

    private void GetAPINameFromGeneralDetailsModelName()
    {
        _generalApiName = typeof(GeneralDetailsModel).Name.Replace("Model", "");
    }

    private void CalculateMaxPages(int count)
    {
        var pages = (double)count / (double)_pageSize;
        var maxPages = (int)Math.Ceiling(pages);
        MaxPages = maxPages;
    }
}