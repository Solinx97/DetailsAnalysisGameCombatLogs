using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class DamageDoneDetailsViewModel : DetailsGenericTemplate<DamageDoneModel, DamageDoneGeneralModel>
{
    private readonly PowerUpInCombat<DamageDoneModel> _powerUpInCombat;

    private ObservableCollection<DamageDoneModel> _damageDoneInformationsWithoutFilter;
    private ObservableCollection<DamageDoneModel> _damageDoneInformationsWithSkipDamage;
    private List<DamageDoneGeneralModel> _allGeneralInformations;
    private List<DamageDoneModel> _allDetailsInformations;

    private bool _isShowCrit = true;
    private bool _isShowDodge = true;
    private bool _isShowParry = true;
    private bool _isShowMiss = true;
    private bool _isShowResist = true;
    private bool _isShowImmune = true;
    private bool _isShowDirectDamage;
    private bool _isShowPets = true;

    public DamageDoneDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache,
        IMapper mapper, ICacheService cacheService) : base(httpClient, logger, memoryCache, mapper, cacheService)
    {
        _powerUpInCombat = new PowerUpInCombat<DamageDoneModel>(_damageDoneInformationsWithSkipDamage);

        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), 3);
    }

    #region Properties

    public bool IsShowCrit
    {
        get { return _isShowCrit; }
        set
        {
            SetProperty(ref _isShowCrit, value);

            _powerUpInCombat.UpdateProperty("IsCrit");
            _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
            DetailsInformations = _powerUpInCombat.ShowSpecificalValue("Time", DetailsInformations, value);

            RaisePropertyChanged(() => DetailsInformations);
        }
    }

    public bool IsShowDodge
    {
        get { return _isShowDodge; }
        set
        {
            SetProperty(ref _isShowDodge, value);

            _powerUpInCombat.UpdateProperty("IsDodge");
            _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
            DetailsInformations = _powerUpInCombat.ShowSpecificalValue("Time", DetailsInformations, value);

            RaisePropertyChanged(() => DetailsInformations);
        }
    }

    public bool IsShowParry
    {
        get { return _isShowParry; }
        set
        {
            SetProperty(ref _isShowParry, value);

            _powerUpInCombat.UpdateProperty("IsParry");
            _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
            DetailsInformations = _powerUpInCombat.ShowSpecificalValue("Time", DetailsInformations, value);

            RaisePropertyChanged(() => DetailsInformations);
        }
    }

    public bool IsShowMiss
    {
        get { return _isShowMiss; }
        set
        {
            SetProperty(ref _isShowMiss, value);

            _powerUpInCombat.UpdateProperty("IsMiss");
            _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
            DetailsInformations = _powerUpInCombat.ShowSpecificalValue("Time", DetailsInformations, value);

            RaisePropertyChanged(() => DetailsInformations);
        }
    }

    public bool IsShowResist
    {
        get { return _isShowResist; }
        set
        {
            SetProperty(ref _isShowResist, value);

            _powerUpInCombat.UpdateProperty("IsResist");
            _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
            DetailsInformations = _powerUpInCombat.ShowSpecificalValue("Time", DetailsInformations, value);

            RaisePropertyChanged(() => DetailsInformations);
        }
    }

    public bool IsShowImmune
    {
        get { return _isShowImmune; }
        set
        {
            SetProperty(ref _isShowImmune, value);

            _powerUpInCombat.UpdateProperty("IsImmune");
            _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
            DetailsInformations = _powerUpInCombat.ShowSpecificalValue("Time", DetailsInformations, value);

            RaisePropertyChanged(() => DetailsInformations);
        }
    }

    public bool IsShowDirectDamage
    {
        get { return _isShowDirectDamage; }
        set
        {
            SetProperty(ref _isShowDirectDamage, value);
        }
    }

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

    protected override void ChildPrepare(CombatPlayerModel parameter)
    {
        var damageDoneCollection = _cacheService.GetDataFromCache<Dictionary<string, List<DamageDone>>>($"{AppCacheKeys.CombatDetails_DamageDone}_{SelectedCombat.LocallyNumber}");
        var damageDoneCollectionMap = _mapper.Map<List<DamageDoneModel>>(damageDoneCollection[parameter.PlayerId]);
        DetailsInformations = new ObservableCollection<DamageDoneModel>(damageDoneCollectionMap);
        _allDetailsInformations = new List<DamageDoneModel>(damageDoneCollectionMap);

        var damageDoneGeneralCollection = _cacheService.GetDataFromCache<Dictionary<string, List<DamageDoneGeneral>>>($"{AppCacheKeys.CombatDetails_DamageDoneGeneral}_{SelectedCombat.LocallyNumber}");
        var damageDoneGeneralCollectionMap = _mapper.Map<List<DamageDoneGeneralModel>>(damageDoneGeneralCollection[parameter.PlayerId]);
        GeneralInformations = new ObservableCollection<DamageDoneGeneralModel>(damageDoneGeneralCollectionMap);
        _allGeneralInformations = new List<DamageDoneGeneralModel>(damageDoneGeneralCollectionMap);
    }

    protected override void Filter()
    {
        DetailsInformations = _damageDoneInformationsWithoutFilter.Any(x => x.Spell == SelectedSource)
            ? new ObservableCollection<DamageDoneModel>(_damageDoneInformationsWithoutFilter.Where(x => x.Spell == SelectedSource))
            : _damageDoneInformationsWithoutFilter;
    }

    protected override async Task LoadCountAsync()
    {
        var count = await _combatParserAPIService.LoadCountAsync($"DamageDone/count/{SelectedPlayerId}");
        Count = count;

        var pages = (double)count / (double)_pageSize;
        var maxPages = (int)Math.Ceiling(pages);
        MaxPages = maxPages;
    }

    protected override async Task LoadDetailsAsync(int page, int pageSize)
    {
        var details = await _combatParserAPIService.LoadCombatDetailsAsync<DamageDoneModel>($"DamageDone/getByCombatPlayerId?combatPlayerId={SelectedPlayerId}&page={page}&pageSize={pageSize}");
        DetailsInformations = new ObservableCollection<DamageDoneModel>(details.ToList());
    }

    protected override async Task LoadGenericDetailsAsync()
    {
        var generalDetails = await _combatParserAPIService.LoadCombatDetailsAsync<DamageDoneGeneralModel>($"DamageDoneGeneral/getByCombatPlayerId/{SelectedPlayerId}");
        GeneralInformations = new ObservableCollection<DamageDoneGeneralModel>(generalDetails.ToList());
    }

    protected override void SetUpFilteredCollection()
    {
        _damageDoneInformationsWithoutFilter = new ObservableCollection<DamageDoneModel>(DetailsInformations);
        _damageDoneInformationsWithSkipDamage = new ObservableCollection<DamageDoneModel>(DetailsInformations);
    }

    protected override void SetTotalValue(CombatPlayerModel parameter)
    {
        TotalValue = parameter.DamageDone;
    }

    protected override void TurnOnAllFilters()
    {
        if (!IsShowDirectDamage) IsShowDirectDamage = true;
        if (!IsShowImmune) IsShowImmune = true;
        if (!IsShowResist) IsShowResist = true;
        if (!IsShowMiss) IsShowMiss = true;
        if (!IsShowParry) IsShowParry = true;
        if (!IsShowDodge) IsShowDodge = true;
        if (!IsShowCrit) IsShowCrit = true;
    }

    private void ShowPets(bool isShowPets)
    {
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