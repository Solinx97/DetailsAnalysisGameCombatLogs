using AutoMapper;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Entities;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using CombatAnalysis.Core.ViewModels.Base;
using Microsoft.Extensions.Caching.Memory;
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
    protected readonly ILogger _logger;
    protected readonly IMapper _mapper;
    protected readonly ICacheService? _cacheService;
    protected readonly CombatParserAPIService? _combatParserAPIService;

    protected List<GeneralDetailsModel>? _allGeneralInformations;
    protected List<DetailsModel>? _allDetailsInformations;

    protected readonly int _pageSize = 20;

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
    private ObservableCollection<string>? spells;
    private int _detailsTypeSelectedIndex;

    public DetailsGenericTemplate(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache,
        IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;

        FirstPageCommand = new MvxAsyncCommand(LoadFirstPageAsync);
        PrevPageCommand = new MvxAsyncCommand(LoadPrevPageAsync);
        NextPageCommand = new MvxAsyncCommand(LoadNextPageAsync);
        LastPageCommand = new MvxAsyncCommand(LoadLastPageAsync);

        _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);
    }

    public DetailsGenericTemplate(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache,
        IMapper mapper, ICacheService cacheService) : this(httpClient, logger, memoryCache, mapper)
    {
        _cacheService = cacheService;
    }

    #region Commands

    public IMvxAsyncCommand FirstPageCommand { get; set; }

    public IMvxAsyncCommand PrevPageCommand { get; set; }

    public IMvxAsyncCommand NextPageCommand { get; set; }

    public IMvxAsyncCommand LastPageCommand { get; set; }

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

            Filter();
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
        get { return spells; }
        set
        {
            SetProperty(ref spells, value);
        }
    }

    #endregion

    public override void Prepare(CombatPlayerModel parameter)
    {
        if (parameter == null)
        {
            return;
        }

        SelectedPlayer = parameter.Username;
        SelectedPlayerId = parameter.Id;

        SetTotalValue(parameter);

        if (SelectedCombat?.Id > 0)
        {
            Task.Run(async () =>
            {
                GetAPINameFromDetailsModelName();
                GetAPINameFromGeneralDetailsModelName();

                await LoadCountAsync();

                await LoadDetailsAsync(Page, _pageSize);
                await LoadGenericDetailsAsync();

                GetSources();
            });
        }
        else
        {
            ExtendedPrepare(parameter);

            GetSources();
        }
    }

    public void GetSources()
    {
        var sources = DetailsInformations?.Select(x => x.Spell).Distinct().ToList();
        if (sources == null)
        {
            return;
        }

        var resourceMangaer = new ResourceManager("CombatAnalysis.App.Localizations.Resources.DetailsGeneralTemplate.Resource", Assembly.Load("CombatAnalysis.App"));
        var allSourcesName = resourceMangaer.GetString("All");
        if (!string.IsNullOrEmpty(allSourcesName))
        {
            sources.Insert(0, allSourcesName);
        }

        Sources = new ObservableCollection<string>(sources);
    }

    protected abstract void ExtendedPrepare(CombatPlayerModel parameter);

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
        if (_combatParserAPIService == null)
        {
            return;
        }

        var count = await _combatParserAPIService.LoadCountAsync($"{_apiName}/count/{SelectedPlayerId}");
        Count = count;

        var pages = (double)count / (double)_pageSize;
        var maxPages = (int)Math.Ceiling(pages);
        MaxPages = maxPages;
    }

    private async Task LoadDetailsAsync(int page, int pageSize)
    {
        if (_combatParserAPIService == null)
        {
            return;
        }

        var details = await _combatParserAPIService.LoadCombatDetailsAsync<DetailsModel>($"{_apiName}/getByCombatPlayerId?combatPlayerId={SelectedPlayerId}&page={page}&pageSize={pageSize}");
        _allDetailsInformations = new List<DetailsModel>(details.ToList());
        DetailsInformations = new ObservableCollection<DetailsModel>(_allDetailsInformations);
    }

    private async Task LoadGenericDetailsAsync()
    {
        if (_combatParserAPIService == null)
        {
            return;
        }

        var generalDetails = await _combatParserAPIService.LoadCombatDetailsAsync<GeneralDetailsModel>($"{_generalApiName}/getByCombatPlayerId/{SelectedPlayerId}");
        _allGeneralInformations = new List<GeneralDetailsModel>(generalDetails.ToList());
        GeneralInformations = new ObservableCollection<GeneralDetailsModel>(_allGeneralInformations);
    }

    private void SetTotalValue(CombatPlayerModel parameter)
    {
        TotalValue = parameter.DamageDone;
    }

    private void Filter()
    {
        if (_allDetailsInformations == null)
        {
            return;
        }

        DetailsInformations = _allDetailsInformations.Any(x => x.Spell == SelectedSource)
            ? new ObservableCollection<DetailsModel>(_allDetailsInformations.Where(x => x.Spell == SelectedSource))
            : new ObservableCollection<DetailsModel>(_allDetailsInformations);
    }

    private void GetAPINameFromDetailsModelName()
    {
        _apiName = typeof(DetailsModel).Name.Replace("Model", "");
    }

    private void GetAPINameFromGeneralDetailsModelName()
    {
        _generalApiName = typeof(GeneralDetailsModel).Name.Replace("Model", "");
    }
}