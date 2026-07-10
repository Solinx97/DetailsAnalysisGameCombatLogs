using AutoMapper;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Entities;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.Models.GameLogs.Details;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.Details;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Resources;

namespace CombatAnalysis.Core.ViewModels.ViewModelTemplates;

public class DetailsGenericTemplateViewModel : ParentTemplate<DetailsGenericModel>
{
    protected readonly IHttpClientHelper _httpClient;
    protected readonly ILogger _logger;
    protected readonly IMapper _mapper;
    protected readonly ICacheService? _cacheService;
    protected readonly ICombatParserAPIService _combatParserAPIService;

    protected List<IGeneralDetailsEntity>? _allGeneralInformations;
    protected List<IDetailsEntity>? _allDetailsInformations;
    protected DetailsGenericModel _detailsGeneric;
    protected Type _listGenericModelType;
    protected Type _listModelType;
    protected CancellationTokenSource _cancelToken;

    protected readonly int _pageSize = 20;

    private int _page = 1;
    private int _count;
    private int _maxPages;

    private string? _selectedCreator;
    private string? _selectedTarget;
    private string? _selectedSpell;
    private string? _selectedPlayer;
    private int _selectedPlayerId;
    private long _totalValue;
    private ObservableCollection<string>? _creators = [];
    private ObservableCollection<string>? _targets = [];
    private ObservableCollection<string>? _spells = [];
    private int _detailsTypeSelectedIndex;
    private LoadingStatus _loadingStatus;

    private ObservableCollection<IGeneralDetailsEntity>? _generalInformations;
    private ObservableCollection<IDetailsEntity>? _detailsInformations;

    public DetailsGenericTemplateViewModel(IHttpClientHelper httpClient, ILogger logger, IMapper mapper,
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

        Basic.Parent = this;
    }

    public DetailsGenericTemplateViewModel(IHttpClientHelper httpClient, ILogger logger, IMapper mapper,
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

    public static CombatModel? SelectedCombat { get; set; }

    public IMvxViewModel<KeyValuePair<ObservableCollection<IDetailsEntity>, ObservableCollection<IGeneralDetailsEntity>>> CurrentView { get; set; }

    #region View model properties

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

    public string? SelectedCreator
    {
        get { return _selectedCreator; }
        set
        {
            SetProperty(ref _selectedCreator, value);
            _ = LoadDetailsAsync(Page, _pageSize);
        }
    }

    public string? SelectedTarget
    {
        get { return _selectedTarget; }
        set
        {
            SetProperty(ref _selectedTarget, value);
            _ = LoadDetailsAsync(Page, _pageSize);
        }
    }

    public string? SelectedSpell
    {
        get { return _selectedSpell; }
        set
        {
            SetProperty(ref _selectedSpell, value);
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

    public int DetailsTypeSelectedIndex
    {
        get { return _detailsTypeSelectedIndex; }
        set
        {
            SetProperty(ref _detailsTypeSelectedIndex, value);
        }
    }

    public ObservableCollection<IDetailsEntity>? DetailsInformations
    {
        get { return _detailsInformations; }
        set
        {
            SetProperty(ref _detailsInformations, value);
        }
    }

    public ObservableCollection<IGeneralDetailsEntity>? GeneralInformations
    {
        get { return _generalInformations; }
        set
        {
            SetProperty(ref _generalInformations, value);
        }
    }

    public ObservableCollection<string>? Creators
    {
        get { return _creators; }
        set
        {
            SetProperty(ref _creators, value);
        }
    }

    public ObservableCollection<string>? Targets
    {
        get { return _targets; }
        set
        {
            SetProperty(ref _targets, value);
        }
    }

    public ObservableCollection<string>? Spells
    {
        get { return _spells; }
        set
        {
            SetProperty(ref _spells, value);
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

    public override void Prepare(DetailsGenericModel parameter)
    {
        _detailsGeneric = parameter;

        var type = Type.GetType(_detailsGeneric.GenericModelType);
        _listGenericModelType = typeof(IEnumerable<>).MakeGenericType(type);

        type = Type.GetType(_detailsGeneric.ModelType);
        _listModelType = typeof(IEnumerable<>).MakeGenericType(type);

        switch (parameter.APIName)
        {
            case "DamageDone":
                CurrentView = new DamageDoneViewModel();
                TotalValue = parameter.CombatPlayer.DamageDone;
                break;
            case "HealDone":
                CurrentView = new HealDoneViewModel();
                TotalValue = parameter.CombatPlayer.HealDone;
                break;
            case "DamageTaken":
                CurrentView = new DamageTakenViewModel();
                TotalValue = parameter.CombatPlayer.DamageTaken;
                break;
            case "ResourceRecovery":
                CurrentView = new ResourceRecoveryViewModel();
                TotalValue = parameter.CombatPlayer.ResourcesRecovery;
                break;
            default:
                break;
        }

        SelectedPlayer = parameter.CombatPlayer.Player.Username;
        SelectedPlayerId = parameter.CombatPlayer.Id;

        base.Prepare();
    }

    public override async Task Initialize()
    {
        _cancelToken = new CancellationTokenSource();

        LoadingStatus = LoadingStatus.Pending;

        await LoadGenericDetailsAsync();
        await LoadDetailsAsync(Page, _pageSize);
        await LoadCountAsync();

        switch (_detailsGeneric.APIName)
        {
            case "DamageDone":
                GetTargets();
                break;
            case "HealDone":
                GetTargets();
                break;
            case "DamageTaken":
                GetCreators();
                break;
            case "ResourceRecovery":
                GetCreators();
                break;
            default:
                break;
        }

        GetSpells();

        LoadingStatus = LoadingStatus.Successful;

        await base.Initialize();
    }

    public override void ViewDestroy(bool viewFinishing = true)
    {
        _cancelToken?.Cancel();

        base.ViewDestroy(viewFinishing);
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

    public async Task LoadGenericDetailsAsync()
    {
        var generalInformations = (IEnumerable<IGeneralDetailsEntity>)await _combatParserAPIService.LoadCombatDetailsAsync(_listGenericModelType, _httpClient, _logger, $"{_detailsGeneric.GenericAPIName}/getByCombatPlayerId/{SelectedPlayerId}", _cancelToken.Token);
        if (generalInformations != null && generalInformations.Any())
        {
            _allGeneralInformations = [.. generalInformations];
            GeneralInformations = new ObservableCollection<IGeneralDetailsEntity>(_allGeneralInformations);
        }
    }

    public async Task LoadDetailsAsync(int page, int pageSize)
    {
        var creator = SelectedCreator;
        if (string.IsNullOrEmpty(creator) || string.Equals(creator, SourcesType.All.ToString()))
        {
            creator = "NONE";
        }

        var target = SelectedTarget;
        if (string.IsNullOrEmpty(target) || string.Equals(target, SourcesType.All.ToString()))
        {
            target = "NONE";
        }

        var source = SelectedSpell;
        if (string.IsNullOrEmpty(source) || string.Equals(source, SourcesType.All.ToString()))
        {
            source = "NONE";
        }

        var detailsInformations = (IEnumerable<IDetailsEntity>)await _combatParserAPIService.LoadCombatDetailsAsync(_listModelType, _httpClient, _logger, $"{_detailsGeneric.APIName}/getAll?combatPlayerId={SelectedPlayerId}&target={target}&creator={creator}&spell={source}&from=00:00:00&to=00:00:00&page={page}&pageSize={pageSize}", _cancelToken.Token);
        if (detailsInformations != null && detailsInformations.Any())
        {
            _allDetailsInformations = [.. detailsInformations];
            DetailsInformations = new ObservableCollection<IDetailsEntity>(_allDetailsInformations);

            var data = new KeyValuePair<ObservableCollection<IDetailsEntity>, ObservableCollection<IGeneralDetailsEntity>>(DetailsInformations, GeneralInformations);
            CurrentView.Prepare(data);
        }
    }

    public async Task LoadCountAsync()
    {
        var creator = SelectedCreator;
        if (string.IsNullOrEmpty(creator) || string.Equals(creator, SourcesType.All.ToString()))
        {
            creator = "NONE";
        }

        var target = SelectedTarget;
        if (string.IsNullOrEmpty(target) || string.Equals(target, SourcesType.All.ToString()))
        {
            target = "NONE";
        }

        var source = SelectedSpell;
        if (string.IsNullOrEmpty(source) || string.Equals(source, SourcesType.All.ToString()))
        {
            source = "NONE";
        }

        var count = await _combatParserAPIService.LoadCountAsync($"{_detailsGeneric.APIName}/count?combatPlayerId={SelectedPlayerId}&target={target}&creator={creator}&spell={source}&from=00:00:00&to=00:00:00", _cancelToken.Token);
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

    public void GetCreators()
    {
        var creators = DetailsInformations?.Select(x => x.Creator).Distinct().ToList();
        if (creators == null)
        {
            return;
        }

        var resourceManager = new ResourceManager("CombatAnalysis.App.Localizations.Resources.DetailsGeneric.Resource", Assembly.Load("CombatAnalysis.App"));
        var allSourcesName = resourceManager.GetString(SourcesType.All.ToString());
        if (!string.IsNullOrEmpty(allSourcesName))
        {
            creators.Insert(0, allSourcesName);
        }

        Creators = new ObservableCollection<string>(creators);
    }

    public void GetTargets()
    {
        var targets = DetailsInformations?.Select(x => x.Target).Distinct().ToList();
        if (targets == null)
        {
            return;
        }

        var resourceManager = new ResourceManager("CombatAnalysis.App.Localizations.Resources.DetailsGeneric.Resource", Assembly.Load("CombatAnalysis.App"));
        var allSourcesName = resourceManager.GetString(SourcesType.All.ToString());
        if (!string.IsNullOrEmpty(allSourcesName))
        {
            targets.Insert(0, allSourcesName);
        }

        Targets = new ObservableCollection<string>(targets);
    }

    public void GetSpells()
    {
        var spells = DetailsInformations?.Select(x => x.Spell).Distinct().ToList();
        if (spells == null)
        {
            return;
        }

        var resourceManager = new ResourceManager("CombatAnalysis.App.Localizations.Resources.DetailsGeneric.Resource", Assembly.Load("CombatAnalysis.App"));
        var allSourcesName = resourceManager.GetString(SourcesType.All.ToString());
        if (!string.IsNullOrEmpty(allSourcesName))
        {
            spells.Insert(0, allSourcesName);
        }

        Spells = new ObservableCollection<string>(spells);
    }

    private void CalculateMaxPages(int count)
    {
        var pages = (double)count / (double)_pageSize;
        var maxPages = (int)Math.Ceiling(pages);
        MaxPages = maxPages;
    }
}