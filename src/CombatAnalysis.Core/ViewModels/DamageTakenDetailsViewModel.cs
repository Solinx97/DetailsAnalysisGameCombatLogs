using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class DamageTakenDetailsViewModel : DetailsGenericTemplate<DamageTakenModel, DamageTakenGeneralModel>
{
    public DamageTakenDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache, 
        IMapper mapper, ICacheService cacheService) : base(httpClient, logger, memoryCache, mapper, cacheService)
    {
        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), 5);
    }

    #region Commands

    public IMvxCommand ShowDamageInformCommand { get; set; }

    #endregion

    protected override void ChildPrepare(CombatPlayerModel parameter)
    {
        var damageTakenCollection = _cacheService.GetDataFromCache<Dictionary<string, List<DamageTaken>>>($"{AppCacheKeys.CombatDetails_DamageTaken}_{SelectedCombat.LocallyNumber}");
        var damageTakenCollectionMap = _mapper.Map<List<DamageTakenModel>>(damageTakenCollection[parameter.PlayerId]);
        DetailsInformations = new ObservableCollection<DamageTakenModel>(damageTakenCollectionMap);
        _allDetailsInformations = new List<DamageTakenModel>(damageTakenCollectionMap);

        var damageTakenGeneralCollection = _cacheService.GetDataFromCache<Dictionary<string, List<DamageTakenGeneral>>>($"{AppCacheKeys.CombatDetails_DamageTakenGeneral}_{SelectedCombat.LocallyNumber}");
        var damageTakenGeneralCollectionMap = _mapper.Map<List<DamageTakenGeneralModel>>(damageTakenGeneralCollection[parameter.PlayerId]);
        GeneralInformations = new ObservableCollection<DamageTakenGeneralModel>(damageTakenGeneralCollectionMap);
        _allGeneralInformations = new List<DamageTakenGeneralModel>(damageTakenGeneralCollectionMap);
    }
}
