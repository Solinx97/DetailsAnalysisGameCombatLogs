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
        var damageTakenCollection = _cacheService.GetDataFromCache<Dictionary<string, List<DamageTaken>>>($"{AppCacheKeys.CombatDetails_DamageTaken}_{SelectedCombat.LocallyNumber}");
        var damageTakenCollectionMap = _mapper.Map<List<DamageTakenModel>>(damageTakenCollection[parameter.PlayerId]);
        DetailsInformations = new ObservableCollection<DamageTakenModel>(damageTakenCollectionMap);
        _allDetailsInformations = new List<DamageTakenModel>(damageTakenCollectionMap);

        var damageTakenGeneralCollection = _cacheService.GetDataFromCache<Dictionary<string, List<DamageTakenGeneral>>>($"{AppCacheKeys.CombatDetails_DamageTakenGeneral}_{SelectedCombat.LocallyNumber}");
        var damageTakenGeneralCollectionMap = _mapper.Map<List<DamageTakenGeneralModel>>(damageTakenGeneralCollection[parameter.PlayerId]);
        GeneralInformations = new ObservableCollection<DamageTakenGeneralModel>(damageTakenGeneralCollectionMap);
        _allGeneralInformations = new List<DamageTakenGeneralModel>(damageTakenGeneralCollectionMap);
    }

    protected override void SetUpFilteredCollection()
    {
        _damageTakenInformationsWithSkipDamage = new ObservableCollection<DamageTakenModel>(DetailsInformations);
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
