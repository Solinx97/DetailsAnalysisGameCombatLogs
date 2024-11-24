using AutoMapper;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using CombatAnalysis.Core.ViewModels.Base;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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

    private bool _isShowFilters;
    private string _selectedDamageDoneSource;
    private string _selectedPlayer;
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

        _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);
    }

    public DetailsGenericTemplate(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache,
        IMapper mapper, ICacheService cacheService) : this(httpClient, logger, memoryCache, mapper)
    {
        _cacheService = cacheService;
    }

    #region Properties

    public static CombatModel SelectedCombat { get; set; }

    public static bool IsNeedSave { get; set; }

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
        SetTotalValue(parameter);

        if (SelectedCombat.Id > 0)
        {
            Task.Run(async () =>
            {
                await LoadDetailsAsync(parameter.Id);
                await LoadGenericDetailsAsync(parameter.Id);

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

        var sources = DetailsInformations.Select(x => x.GetType().GetProperty("SpellOrItem").GetValue(x).ToString()).Distinct().ToList();
        var resourceMangaer = new ResourceManager("CombatAnalysis.App.Localizations.Resources.DetailsGeneralTemplate.Resource", Assembly.Load("CombatAnalysis.App"));
        var allSourcesName = resourceMangaer.GetString("All");
        sources.Insert(0, allSourcesName);

        Sources = new ObservableCollection<string>(sources);
    }

    protected abstract void Filter();

    protected abstract Task LoadDetailsAsync(int combatPlayerId);

    protected abstract Task LoadGenericDetailsAsync(int combatPlayerId);

    protected abstract void TurnOnAllFilters();

    protected abstract void SetUpFilteredCollection();

    protected abstract void SetTotalValue(CombatPlayerModel parameter);
}