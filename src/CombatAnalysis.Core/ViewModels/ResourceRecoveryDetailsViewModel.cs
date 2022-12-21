using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class ResourceRecoveryDetailsViewModel : GenericTemplate<Tuple<CombatPlayerModel, CombatModel>>
{
    private readonly CombatParserAPIService _combatParserAPIService;

    private ObservableCollection<ResourceRecoveryModel> _resourceRecoveryInformations;
    private ObservableCollection<ResourceRecoveryModel> _resourceRecoveryInformationsWithoutFilter;
    private ObservableCollection<string> _resourceRecoverySources;
    private ObservableCollection<ResourceRecoveryGeneralModel> _resourceRecoveryGeneralInformations;

    public ResourceRecoveryDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache)
    {
        _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);

        BasicTemplate = Templates.Basic;
        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 6);
    }

    #region Properties

    public ObservableCollection<ResourceRecoveryModel> ResourceRecoveryInformations
    {
        get { return _resourceRecoveryInformations; }
        set
        {
            SetProperty(ref _resourceRecoveryInformations, value);
        }
    }

    public ObservableCollection<string> ResourceRecoverySources
    {
        get { return _resourceRecoverySources; }
        set
        {
            SetProperty(ref _resourceRecoverySources, value);
        }
    }

    public ObservableCollection<ResourceRecoveryGeneralModel> ResourceRecoveryGeneralInformations
    {
        get { return _resourceRecoveryGeneralInformations; }
        set
        {
            SetProperty(ref _resourceRecoveryGeneralInformations, value);
        }
    }

    #endregion

    protected override async Task ChildPrepareAsync(Tuple<CombatPlayerModel, CombatModel> parameter)
    {
        var combat = parameter.Item2;
        var player = parameter.Item1;
        SelectedPlayer = player.UserName;
        TotalValue = player.EnergyRecovery;

        await LoadDetailsAsync(player.Id);
        await LoadGenericDetailsAsync(player.Id);
    }

    protected override void Filter()
    {
        ResourceRecoveryInformations = _resourceRecoveryInformationsWithoutFilter.Any(x => x.SpellOrItem == SelectedSource)
            ? new ObservableCollection<ResourceRecoveryModel>(_resourceRecoveryInformationsWithoutFilter.Where(x => x.SpellOrItem == SelectedSource))
            : _resourceRecoveryInformationsWithoutFilter;
    }

    protected override async Task LoadDetailsAsync(int combatPlayerId)
    {
        var details = await _combatParserAPIService.LoadResourceRecoveryDetailsAsync(combatPlayerId);
        ResourceRecoveryInformations = new ObservableCollection<ResourceRecoveryModel>(details.ToList());

        GetDetails();
    }

    protected override async Task LoadGenericDetailsAsync(int combatPlayerId)
    {
        var generalDetails = await _combatParserAPIService.LoadResourceRecoveryGeneralAsync(combatPlayerId);
        ResourceRecoveryGeneralInformations = new ObservableCollection<ResourceRecoveryGeneralModel>(generalDetails.ToList());
    }

    protected override void GetDetails()
    {
        _resourceRecoveryInformations = new ObservableCollection<ResourceRecoveryModel>(ResourceRecoveryInformations);
        _resourceRecoveryInformationsWithoutFilter = new ObservableCollection<ResourceRecoveryModel>(ResourceRecoveryInformations);

        var sources = ResourceRecoveryInformations.Select(x => x.SpellOrItem).Distinct().ToList();
        sources.Insert(0, "Все");
        ResourceRecoverySources = new ObservableCollection<string>(sources);
    }

    protected override void TurnOnAllFilters()
    {
        // write here your filters
    }
}
