using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
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

    protected override void ChildPrepare(Tuple<CombatPlayerModel, CombatModel> parameter)
    {
        var combat = parameter.Item2;
        var player = parameter.Item1;
        SelectedPlayer = player.UserName;
        TotalValue = player.EnergyRecovery;

        Task.Run(async () => await LoadResourceRecoveryDetailsAsync(player.Id));
        Task.Run(async () => await LoadResourceRecoveryGeneralAsync(player.Id));
    }

    protected override void GetDetails()
    {
        _resourceRecoveryInformations = new ObservableCollection<ResourceRecoveryModel>(ResourceRecoveryInformations);
        _resourceRecoveryInformationsWithoutFilter = new ObservableCollection<ResourceRecoveryModel>(ResourceRecoveryInformations);

        var resourceRecoverySources = ResourceRecoveryInformations.Select(x => x.SpellOrItem).Distinct().ToList();
        resourceRecoverySources.Insert(0, "Все");
        ResourceRecoverySources = new ObservableCollection<string>(resourceRecoverySources);
    }

    protected override void Filter()
    {
        ResourceRecoveryInformations = _resourceRecoveryInformationsWithoutFilter.Any(x => x.SpellOrItem == SelectedSource)
            ? new ObservableCollection<ResourceRecoveryModel>(_resourceRecoveryInformationsWithoutFilter.Where(x => x.SpellOrItem == SelectedSource))
            : _resourceRecoveryInformationsWithoutFilter;
    }

    private async Task LoadResourceRecoveryDetailsAsync(int combatPlayerId)
    {
        var resourceRecoveries = await _combatParserAPIService.LoadResourceRecoveryDetailsAsync(combatPlayerId);
        ResourceRecoveryInformations = new ObservableCollection<ResourceRecoveryModel>(resourceRecoveries.ToList());
        _resourceRecoveryInformations = new ObservableCollection<ResourceRecoveryModel>(resourceRecoveries.ToList());
    }

    private async Task LoadResourceRecoveryGeneralAsync(int combatPlayerId)
    {
        var resourceRecoveries = await _combatParserAPIService.LoadResourceRecoveryGeneralAsync(combatPlayerId);
        ResourceRecoveryGeneralInformations = new ObservableCollection<ResourceRecoveryGeneralModel>(resourceRecoveries.ToList());
    }
}
