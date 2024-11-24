using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class ResourceRecoveryDetailsViewModel : DetailsGenericTemplate<ResourceRecoveryModel, ResourceRecoveryGeneralModel>
{
    private ObservableCollection<ResourceRecoveryModel> _resourceRecoveryInformationsWithoutFilter;

    public ResourceRecoveryDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache, 
        IMapper mapper, ICacheService cacheService) : base(httpClient, logger, memoryCache, mapper, cacheService)
    {
        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), 6);
    }

    protected override void ChildPrepare(CombatPlayerModel parameter)
    {
        var resourcesRecoveryCollection = _cacheService.GetDataFromCache<Dictionary<string, List<ResourceRecovery>>>($"{AppCacheKeys.CombatDetails_ResourcesRecovery}_{SelectedCombat.LocallyNumber}");
        var resourcesRecoveryCollectionMap = _mapper.Map<List<ResourceRecoveryModel>>(resourcesRecoveryCollection[parameter.PlayerId]);
        DetailsInformations = new ObservableCollection<ResourceRecoveryModel>(resourcesRecoveryCollectionMap);

        var resourcesRecoveryGeneralCollection = _cacheService.GetDataFromCache<Dictionary<string, List<ResourceRecoveryGeneral>>>($"{AppCacheKeys.CombatDetails_ResourcesRecoveryGeneral}_{SelectedCombat.LocallyNumber}");
        var resourcesRecoveryGeneralCollectionMap = _mapper.Map<List<ResourceRecoveryGeneralModel>>(resourcesRecoveryGeneralCollection[parameter.PlayerId]);
        GeneralInformations = new ObservableCollection<ResourceRecoveryGeneralModel>(resourcesRecoveryGeneralCollectionMap);
    }

    protected override void Filter()
    {
        DetailsInformations = _resourceRecoveryInformationsWithoutFilter.Any(x => x.SpellOrItem == SelectedSource)
            ? new ObservableCollection<ResourceRecoveryModel>(_resourceRecoveryInformationsWithoutFilter.Where(x => x.SpellOrItem == SelectedSource))
            : _resourceRecoveryInformationsWithoutFilter;
    }

    protected override async Task LoadDetailsAsync(int combatPlayerId)
    {
        var details = await _combatParserAPIService.LoadCombatDetailsByCombatPlayerId<ResourceRecoveryModel>("ResourceRecovery/FindByCombatPlayerId", combatPlayerId);
        DetailsInformations = new ObservableCollection<ResourceRecoveryModel>(details.ToList());
    }

    protected override async Task LoadGenericDetailsAsync(int combatPlayerId)
    {
        var generalDetails = await _combatParserAPIService.LoadCombatDetailsByCombatPlayerId<ResourceRecoveryGeneralModel>("ResourceRecoveryGeneral/FindByCombatPlayerId", combatPlayerId);
        GeneralInformations = new ObservableCollection<ResourceRecoveryGeneralModel>(generalDetails.ToList());
    }

    protected override void SetUpFilteredCollection()
    {
        _resourceRecoveryInformationsWithoutFilter = new ObservableCollection<ResourceRecoveryModel>(DetailsInformations);
    }

    protected override void SetTotalValue(CombatPlayerModel parameter)
    {
        TotalValue = parameter.ResourcesRecovery;
    }

    protected override void TurnOnAllFilters()
    {
        // write here your filters
    }
}
