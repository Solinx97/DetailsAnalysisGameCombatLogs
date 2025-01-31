using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class DamageDoneDetailsViewModel : DetailsGenericTemplate<DamageDoneModel, DamageDoneGeneralModel>
{
    private bool _isShowPets = true;

    public DamageDoneDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMapper mapper,
        ICacheService cacheService, ICombatParserAPIService combatParserAPIService) : base(httpClient, logger, mapper, cacheService, combatParserAPIService)
    {
        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 3);
    }

    #region Properties

    public bool IsShowPets
    {
        get { return _isShowPets; }
        set
        {
            SetProperty(ref _isShowPets, value);
            ShowPets(value);
        }
    }

    #endregion

    protected override void ExtendedPrepare(CombatPlayerModel parameter)
    {
        var damageDoneCollection = _cacheService?.GetDataFromCache<Dictionary<string, List<DamageDone>>>($"{AppCacheKeys.CombatDetails_DamageDone}_{SelectedCombat?.LocallyNumber}");
        var damageDoneCollectionMap = _mapper.Map<List<DamageDoneModel>>(damageDoneCollection?[parameter.PlayerId]);
        DetailsInformations = new ObservableCollection<DamageDoneModel>(damageDoneCollectionMap);
        _allDetailsInformations = new List<DamageDoneModel>(damageDoneCollectionMap);

        var damageDoneGeneralCollection = _cacheService?.GetDataFromCache<Dictionary<string, List<DamageDoneGeneral>>>($"{AppCacheKeys.CombatDetails_DamageDoneGeneral}_{SelectedCombat?.LocallyNumber}");
        var damageDoneGeneralCollectionMap = _mapper.Map<List<DamageDoneGeneralModel>>(damageDoneGeneralCollection?[parameter.PlayerId]);
        GeneralInformations = new ObservableCollection<DamageDoneGeneralModel>(damageDoneGeneralCollectionMap);
        _allGeneralInformations = new List<DamageDoneGeneralModel>(damageDoneGeneralCollectionMap);
    }

    private void ShowPets(bool isShowPets)
    {
        if (_allGeneralInformations == null || _allDetailsInformations == null)
        {
            return;
        }

        if (!isShowPets)
        {
            var generalWithoutPets = _allGeneralInformations.Where(x => !x.IsPet);
            GeneralInformations = new ObservableCollection<DamageDoneGeneralModel>(generalWithoutPets);

            var detailsWithoutPets = _allDetailsInformations.Where(x => !x.IsPet);
            DetailsInformations = new ObservableCollection<DamageDoneModel>(detailsWithoutPets);
        }
        else
        {
            GeneralInformations = new ObservableCollection<DamageDoneGeneralModel>(_allGeneralInformations);
            DetailsInformations = new ObservableCollection<DamageDoneModel>(_allDetailsInformations);
        }

        TotalValue = GeneralInformations.Sum(x => x.Value);

        GetSources();
    }
}