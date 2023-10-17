using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Patterns;
using CombatAnalysis.Core.Core;
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
    private Dictionary<string, List<string>> _petsId;

    private bool _isShowCrit = true;
    private bool _isShowDodge = true;
    private bool _isShowParry = true;
    private bool _isShowMiss = true;
    private bool _isShowResist = true;
    private bool _isShowImmune = true;
    private bool _isShowDirectDamage;

    public DamageDoneDetailsViewModel(IHttpClientHelper httpClient, ILogger loger, IMemoryCache memoryCache, IMapper mapper) : base(httpClient, loger, memoryCache, mapper)
    {
        _powerUpInCombat = new PowerUpInCombat<DamageDoneModel>(_damageDoneInformationsWithSkipDamage);

        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), 3);

        _petsId = ((BasicTemplateViewModel)BasicTemplate).PetsId;
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

    #endregion

    protected override void ChildPrepare(CombatPlayerModel parameter)
    {
        var selectedCombatMap = _mapper.Map<Combat>(SelectedCombat);

        var damageDpneDetails = new CombatDetailsDamageDone(_logger)
        {
            PetsId = _petsId,
        };
        damageDpneDetails.GetData(parameter.PlayerId, SelectedCombat.Data);

        var damageDoneMap = _mapper.Map<List<DamageDoneModel>>(damageDpneDetails.DamageDone);
        DetailsInformations = new ObservableCollection<DamageDoneModel>(damageDoneMap);

        var damageDoneGeneralData = damageDpneDetails.GetDamageDoneGeneral(damageDpneDetails.DamageDone, selectedCombatMap);
        var damageDoneGeneralMap = _mapper.Map<List<DamageDoneGeneralModel>>(damageDoneGeneralData);
        GeneralInformations = new ObservableCollection<DamageDoneGeneralModel>(damageDoneGeneralMap);
    }

    protected override void Filter()
    {
        DetailsInformations = _damageDoneInformationsWithoutFilter.Any(x => x.SpellOrItem == SelectedSource)
            ? new ObservableCollection<DamageDoneModel>(_damageDoneInformationsWithoutFilter.Where(x => x.SpellOrItem == SelectedSource))
            : _damageDoneInformationsWithoutFilter;
    }

    protected override async Task LoadDetailsAsync(int combatPlayerId)
    {
        var details = await _combatParserAPIService.LoadCombatDetailsByCombatPlayerId<DamageDoneModel>("DamageDone/FindByCombatPlayerId", combatPlayerId);
        DetailsInformations = new ObservableCollection<DamageDoneModel>(details.ToList());
    }

    protected override async Task LoadGenericDetailsAsync(int combatPlayerId)
    {
        var generalDetails = await _combatParserAPIService.LoadCombatDetailsByCombatPlayerId<DamageDoneGeneralModel>("DamageDoneGeneral/FindByCombatPlayerId", combatPlayerId);
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
}