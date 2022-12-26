using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Localizations;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels;

public class DamageDoneDetailsViewModel : GenericTemplate<CombatPlayerModel>
{
    private readonly PowerUpInCombat<DamageDoneModel> _powerUpInCombat;
    private readonly CombatParserAPIService _combatParserAPIService;

    private ObservableCollection<DamageDoneModel> _damageDoneInformations;
    private ObservableCollection<DamageDoneModel> _damageDoneInformationsWithoutFilter;
    private ObservableCollection<DamageDoneModel> _damageDoneInformationsWithSkipDamage;
    private ObservableCollection<string> _damageDoneSources;
    private ObservableCollection<DamageDoneGeneralModel> _damageDoneGeneralInformations;

    private bool _isShowCrit = true;
    private bool _isShowDodge = true;
    private bool _isShowParry = true;
    private bool _isShowMiss = true;
    private bool _isShowResist = true;
    private bool _isShowImmune = true;
    private bool _isShowDirectDamage;

    public DamageDoneDetailsViewModel(IHttpClientHelper httpClient, ILogger loger, IMemoryCache memoryCache)
    {
        _combatParserAPIService = new CombatParserAPIService(httpClient, loger, memoryCache);
        _powerUpInCombat = new PowerUpInCombat<DamageDoneModel>(_damageDoneInformationsWithSkipDamage);

        BasicTemplate = Templates.Basic;
        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 3);
    }

    #region Properties

    public ObservableCollection<DamageDoneModel> DamageDoneInformations
    {
        get { return _damageDoneInformations; }
        set
        {
            SetProperty(ref _damageDoneInformations, value);
        }
    }

    public ObservableCollection<string> DamageDoneSources
    {
        get { return _damageDoneSources; }
        set
        {
            SetProperty(ref _damageDoneSources, value);
        }
    }

    public ObservableCollection<DamageDoneGeneralModel> DamageDoneGeneralInformations
    {
        get { return _damageDoneGeneralInformations; }
        set
        {
            SetProperty(ref _damageDoneGeneralInformations, value);
        }
    }

    public bool IsShowCrit
    {
        get { return _isShowCrit; }
        set
        {
            SetProperty(ref _isShowCrit, value);

            _powerUpInCombat.UpdateProperty("IsCrit");
            _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
            DamageDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageDoneInformations, value);

            RaisePropertyChanged(() => DamageDoneInformations);
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
            DamageDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageDoneInformations, value);

            RaisePropertyChanged(() => DamageDoneInformations);
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
            DamageDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageDoneInformations, value);

            RaisePropertyChanged(() => DamageDoneInformations);
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
            DamageDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageDoneInformations, value);

            RaisePropertyChanged(() => DamageDoneInformations);
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
            DamageDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageDoneInformations, value);

            RaisePropertyChanged(() => DamageDoneInformations);
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
            DamageDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageDoneInformations, value);

            RaisePropertyChanged(() => DamageDoneInformations);
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

    protected override async Task ChildPrepareAsync(CombatPlayerModel parameter)
    {
        var player = parameter;
        SelectedPlayer = player.UserName;
        TotalValue = player.DamageDone;

        await LoadDetailsAsync(player.Id);
        await LoadGenericDetailsAsync(player.Id);
    }

    protected override void Filter()
    {
        DamageDoneInformations = _damageDoneInformationsWithoutFilter.Any(x => x.SpellOrItem == SelectedSource)
            ? new ObservableCollection<DamageDoneModel>(_damageDoneInformationsWithoutFilter.Where(x => x.SpellOrItem == SelectedSource))
            : _damageDoneInformationsWithoutFilter;
    }

    protected override async Task LoadDetailsAsync(int combatPlayerId)
    {
        var details = await _combatParserAPIService.LoadDamageDoneDetailsAsync(combatPlayerId);
        DamageDoneInformations = new ObservableCollection<DamageDoneModel>(details.ToList());

        GetDetails();
    }

    protected override async Task LoadGenericDetailsAsync(int combatPlayerId)
    {
        var generalDetails = await _combatParserAPIService.LoadDamageDoneGeneralAsync(combatPlayerId);
        DamageDoneGeneralInformations = new ObservableCollection<DamageDoneGeneralModel>(generalDetails.ToList());
    }

    protected override void GetDetails()
    {
        _damageDoneInformationsWithoutFilter = new ObservableCollection<DamageDoneModel>(DamageDoneInformations);
        _damageDoneInformationsWithSkipDamage = new ObservableCollection<DamageDoneModel>(DamageDoneInformations);

        var sources = DamageDoneInformations.Select(x => x.SpellOrItem).Distinct().ToList();
        var allSourcesName = TranslationSource.Instance["CombatAnalysis.App.Localizations.Resources.DamageDoneDetails.Resource.All"];
        sources.Insert(0, allSourcesName);
        DamageDoneSources = new ObservableCollection<string>(sources);
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