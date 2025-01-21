using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class HealDoneDetailsViewModel : DetailsGenericTemplate<HealDoneModel, HealDoneGeneralModel>
{
    public HealDoneDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMapper mapper,
        ICacheService cacheService, ICombatParserAPIService combatParserAPIService) : base(httpClient, logger, mapper, cacheService, combatParserAPIService)
    {
        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 4);
    }

    protected override void ExtendedPrepare(CombatPlayerModel parameter)
    {
        var healDoneCollection = _cacheService?.GetDataFromCache<Dictionary<string, List<HealDone>>>($"{AppCacheKeys.CombatDetails_HealDone}_{SelectedCombat?.LocallyNumber}");
        var healDoneCollectionMap = _mapper.Map<List<HealDoneModel>>(healDoneCollection?[parameter.PlayerId]);
        DetailsInformations = new ObservableCollection<HealDoneModel>(healDoneCollectionMap);
        _allDetailsInformations = new List<HealDoneModel>(healDoneCollectionMap);

        var healDoneGeneralCollection = _cacheService?.GetDataFromCache<Dictionary<string, List<HealDoneGeneral>>>($"{AppCacheKeys.CombatDetails_HealDoneGeneral}_{SelectedCombat?.LocallyNumber}");
        var healDoneGeneralCollectionMap = _mapper.Map<List<HealDoneGeneralModel>>(healDoneGeneralCollection?[parameter.PlayerId]);
        GeneralInformations = new ObservableCollection<HealDoneGeneralModel>(healDoneGeneralCollectionMap);
        _allGeneralInformations = new List<HealDoneGeneralModel>(healDoneGeneralCollectionMap);
    }
}
