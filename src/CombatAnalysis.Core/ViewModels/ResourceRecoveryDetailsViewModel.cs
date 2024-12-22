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
    public ResourceRecoveryDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache, 
        IMapper mapper, ICacheService cacheService) : base(httpClient, logger, memoryCache, mapper, cacheService)
    {
        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 6);
    }

    protected override void ExtendedPrepare(CombatPlayerModel parameter)
    {
        var resourcesRecoveryCollection = _cacheService.GetDataFromCache<Dictionary<string, List<ResourceRecovery>>>($"{AppCacheKeys.CombatDetails_ResourcesRecovery}_{SelectedCombat.LocallyNumber}");
        var resourcesRecoveryCollectionMap = _mapper.Map<List<ResourceRecoveryModel>>(resourcesRecoveryCollection[parameter.PlayerId]);
        DetailsInformations = new ObservableCollection<ResourceRecoveryModel>(resourcesRecoveryCollectionMap);
        _allDetailsInformations = new List<ResourceRecoveryModel>(resourcesRecoveryCollectionMap);

        var resourcesRecoveryGeneralCollection = _cacheService.GetDataFromCache<Dictionary<string, List<ResourceRecoveryGeneral>>>($"{AppCacheKeys.CombatDetails_ResourcesRecoveryGeneral}_{SelectedCombat.LocallyNumber}");
        var resourcesRecoveryGeneralCollectionMap = _mapper.Map<List<ResourceRecoveryGeneralModel>>(resourcesRecoveryGeneralCollection[parameter.PlayerId]);
        GeneralInformations = new ObservableCollection<ResourceRecoveryGeneralModel>(resourcesRecoveryGeneralCollectionMap);
        _allGeneralInformations = new List<ResourceRecoveryGeneralModel>(resourcesRecoveryGeneralCollectionMap);
    }
}
