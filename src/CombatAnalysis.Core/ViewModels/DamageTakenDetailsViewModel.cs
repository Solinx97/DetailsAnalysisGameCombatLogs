using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.Core.Core;
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
    private readonly PowerUpInCombat<DamageTakenModel> _powerUpInCombat;

    private ObservableCollection<DamageTakenModel> _damageTakenInformationsWithoutFilter;
    private ObservableCollection<DamageTakenModel> _damageTakenInformationsWithSkipDamage;

    private bool _isShowDodge = true;
    private bool _isShowParry = true;
    private bool _isShowMiss = true;
    private bool _isShowResist = true;
    private bool _isShowImmune = true;
    private bool _isShowCrushing = true;
    private bool _isShowAbsorb = true;
    private bool _isShowDamageInform = true;
    private bool _IsShowAbsorbed = true;

    public DamageTakenDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache, 
        IMapper mapper, ICacheService cacheService) : base(httpClient, logger, memoryCache, mapper, cacheService)
    {
        _powerUpInCombat = new PowerUpInCombat<DamageTakenModel>(_damageTakenInformationsWithSkipDamage);

        ShowDamageInformCommand = new MvxCommand(() => IsShowDamageInfrom = !IsShowDamageInfrom);

        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), 5);
    }

    #region Commands

    public IMvxCommand ShowDamageInformCommand { get; set; }

    #endregion

    #region Properties

    public bool IsShowDodge
    {
        get { return _isShowDodge; }
        set
        {
            SetProperty(ref _isShowDodge, value);

            _powerUpInCombat.UpdateProperty("IsDodge");
            _powerUpInCombat.UpdateCollection(_damageTakenInformationsWithSkipDamage);
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
            _powerUpInCombat.UpdateCollection(_damageTakenInformationsWithSkipDamage);
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
            _powerUpInCombat.UpdateCollection(_damageTakenInformationsWithSkipDamage);
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
            _powerUpInCombat.UpdateCollection(_damageTakenInformationsWithSkipDamage);
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
            _powerUpInCombat.UpdateCollection(_damageTakenInformationsWithSkipDamage);
            DetailsInformations = _powerUpInCombat.ShowSpecificalValue("Time", DetailsInformations, value);

            RaisePropertyChanged(() => DetailsInformations);
        }
    }

    public bool IsShowCrushing
    {
        get { return _isShowCrushing; }
        set
        {
            SetProperty(ref _isShowCrushing, value);

            _powerUpInCombat.UpdateProperty("IsCrushing");
            _powerUpInCombat.UpdateCollection(_damageTakenInformationsWithSkipDamage);
            DetailsInformations = _powerUpInCombat.ShowSpecificalValue("Time", DetailsInformations, value);

            RaisePropertyChanged(() => DetailsInformations);
        }
    }

    public bool IsShowAbsorb
    {
        get { return _isShowAbsorb; }
        set
        {
            SetProperty(ref _isShowAbsorb, value);

            _powerUpInCombat.UpdateProperty("IsAbsorb");
            _powerUpInCombat.UpdateCollection(_damageTakenInformationsWithSkipDamage);
            DetailsInformations = _powerUpInCombat.ShowSpecificalValue("Time", DetailsInformations, value);

            RaisePropertyChanged(() => DetailsInformations);
        }
    }

    public bool IsShowDamageInfrom
    {
        get { return _isShowDamageInform; }
        set
        {
            SetProperty(ref _isShowDamageInform, value);

            RaisePropertyChanged(() => DetailsInformations);
        }
    }

    public bool IsShowAbsorbed
    {
        get { return _IsShowAbsorbed; }
        set
        {
            SetProperty(ref _IsShowAbsorbed, value);

            if (value)
            {
                TotalValue = GeneralInformations.Sum(x => x.ActualValue);
            }
            else
            {
                TotalValue = GeneralInformations.Sum(x => x.Value);
            }
        }
    }

    #endregion

    protected override void ChildPrepare(CombatPlayerModel parameter)
    {
        var damageDoneCollection = _cacheService.GetDataFromCache<Dictionary<string, List<DamageTaken>>>($"{AppCacheKeys.CombatDetails_DamageTaken}_{SelectedCombat.LocallyNumber}");
        var damageDoneCollectionMap = _mapper.Map<List<DamageTakenModel>>(damageDoneCollection[parameter.PlayerId]);
        DetailsInformations = new ObservableCollection<DamageTakenModel>(damageDoneCollectionMap);

        var damageDoneGeneralCollection = _cacheService.GetDataFromCache<Dictionary<string, List<DamageTakenGeneral>>>($"{AppCacheKeys.CombatDetails_DamageTakenGeneral}_{SelectedCombat.LocallyNumber}");
        var damageDoneGeneralCollectionMap = _mapper.Map<List<DamageTakenGeneralModel>>(damageDoneGeneralCollection[parameter.PlayerId]);
        GeneralInformations = new ObservableCollection<DamageTakenGeneralModel>(damageDoneGeneralCollectionMap);
    }

    protected override void Filter()
    {
        DetailsInformations = _damageTakenInformationsWithoutFilter.Any(x => x.Spell == SelectedSource)
            ? new ObservableCollection<DamageTakenModel>(_damageTakenInformationsWithoutFilter.Where(x => x.Spell == SelectedSource))
            : _damageTakenInformationsWithoutFilter;
    }

    protected override async Task LoadCountAsync()
    {
        var count = await _combatParserAPIService.LoadCountAsync($"DamageTaken/count/{SelectedPlayerId}");
        Count = count;

        var pages = (double)count / (double)_pageSize;
        var maxPages = (int)Math.Ceiling(pages);
        MaxPages = maxPages;
    }

    protected override async Task LoadDetailsAsync(int page, int pageSize)
    {
        var details = await _combatParserAPIService.LoadCombatDetailsAsync<DamageTakenModel>($"DamageTaken/getByCombatPlayerId?combatPlayerId={SelectedPlayerId}&page={page}&pageSize={pageSize}");
        DetailsInformations = new ObservableCollection<DamageTakenModel>(details.ToList());
    }

    protected override async Task LoadGenericDetailsAsync()
    {
        var generalDetails = await _combatParserAPIService.LoadCombatDetailsAsync<DamageTakenGeneralModel>($"DamageTakenGeneral/getByCombatPlayerId/{SelectedPlayerId}");
        GeneralInformations = new ObservableCollection<DamageTakenGeneralModel>(generalDetails.ToList());
    }

    protected override void SetUpFilteredCollection()
    {
        _damageTakenInformationsWithSkipDamage = new ObservableCollection<DamageTakenModel>(DetailsInformations);
        _damageTakenInformationsWithoutFilter = new ObservableCollection<DamageTakenModel>(DetailsInformations);
    }

    protected override void SetTotalValue(CombatPlayerModel parameter)
    {
        TotalValue = parameter.DamageTaken;
    }

    protected override void TurnOnAllFilters()
    {
        if (!IsShowDodge) IsShowDodge = true;
        if (!IsShowParry) IsShowParry = true;
        if (!IsShowMiss) IsShowMiss = true;
        if (!IsShowResist) IsShowResist = true;
        if (!IsShowImmune) IsShowImmune = true;
        if (!IsShowCrushing) IsShowCrushing = true;
        if (!IsShowAbsorb) IsShowAbsorb = true;
    }
}
