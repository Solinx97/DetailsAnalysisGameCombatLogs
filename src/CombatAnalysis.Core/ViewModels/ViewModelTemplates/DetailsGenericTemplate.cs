using AutoMapper;
using CombatAnalysis.Core.Interfaces;
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

abstract public class DetailsGenericTemplate<T, T1> : ParentTemplate<CombatPlayerModel>
    where T : class
    where T1 : class
{
    protected readonly ILogger _logger;
    protected readonly IMapper _mapper;
    protected readonly ICacheService _cacheService;
    protected readonly CombatParserAPIService _combatParserAPIService;

    protected readonly int _pageSize = 20;

    private int _page = 1;
    private int _count;
    private int _maxPages;

    private bool _isShowFilters;
    private string _selectedDamageDoneSource;
    private string _selectedPlayer;
    private int _selectedPlayerId;
    private long _totalValue;
    private ObservableCollection<T> _detailsInformations;
    private ObservableCollection<T1> _generalInformations;
    private ObservableCollection<string> _sources;
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

    public static CombatModel SelectedCombat { get; set; }

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

    public string SelectedSource
    {
        get { return _selectedDamageDoneSource; }
        set
        {
            SetProperty(ref _selectedDamageDoneSource, value);

            Filter();
        }
    }

    public string SelectedPlayer
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

            if (!value)
            {
                TurnOnAllFilters();
            }
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

    public ObservableCollection<T> DetailsInformations
    {
        get { return _detailsInformations; }
        set
        {
            SetProperty(ref _detailsInformations, value);
        }
    }

    public ObservableCollection<T1> GeneralInformations
    {
        get { return _generalInformations; }
        set
        {
            SetProperty(ref _generalInformations, value);
        }
    }

    public ObservableCollection<string> Sources
    {
        get { return _sources; }
        set
        {
            SetProperty(ref _sources, value);
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

        if (SelectedCombat.Id > 0)
        {
            Task.Run(async () =>
            {
                await LoadCountAsync();

                await LoadDetailsAsync(Page, _pageSize);
                await LoadGenericDetailsAsync();

                GetSources();
            });
        }
        else
        {
            ChildPrepare(parameter);
            GetSources();
        }
    }

    public void GetSources()
    {
        SetUpFilteredCollection();

        var sources = DetailsInformations.Select(x => x.GetType().GetProperty("Spell").GetValue(x).ToString()).Distinct().ToList();
        var resourceMangaer = new ResourceManager("CombatAnalysis.App.Localizations.Resources.DetailsGeneralTemplate.Resource", Assembly.Load("CombatAnalysis.App"));
        var allSourcesName = resourceMangaer.GetString("All");
        sources.Insert(0, allSourcesName);

        Sources = new ObservableCollection<string>(sources);
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

    protected abstract void Filter();

    protected abstract Task LoadCountAsync();

    protected abstract Task LoadDetailsAsync(int pgae, int pageSize);

    protected abstract Task LoadGenericDetailsAsync();

    protected abstract void TurnOnAllFilters();

    protected abstract void SetUpFilteredCollection();

    protected abstract void SetTotalValue(CombatPlayerModel parameter);
}