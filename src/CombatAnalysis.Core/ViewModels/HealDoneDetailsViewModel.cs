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

public class HealDoneDetailsViewModel : DetailsGenericTemplate<HealDoneModel, HealDoneGeneralModel>
{
    private readonly PowerUpInCombat<HealDoneModel> _powerUpInCombat;

    private ObservableCollection<HealDoneModel> _healDoneInformationsWithoutFilter;
    private ObservableCollection<HealDoneModel> _healDoneInformationsWithOverheal;

    private bool _isShowOverheal = true;
    private bool _isShowCrit = true;

    public HealDoneDetailsViewModel(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache, 
        IMapper mapper, ICacheService cacheService) : base(httpClient, logger, memoryCache, mapper, cacheService)
    {
        _powerUpInCombat = new PowerUpInCombat<HealDoneModel>(_healDoneInformationsWithOverheal);

        BasicTemplate.Parent = this;
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Step), 4);
    }

    #region Properties

    public bool IsShowOverheal
    {
        get { return _isShowOverheal; }
        set
        {
            SetProperty(ref _isShowOverheal, value);

            _powerUpInCombat.UpdateProperty("IsFullOverheal");
            _powerUpInCombat.UpdateCollection(_healDoneInformationsWithOverheal);
            DetailsInformations = _powerUpInCombat.ShowSpecificalValue("Time", DetailsInformations, value);

            RaisePropertyChanged(() => DetailsInformations);
        }
    }

    public bool IsShowCrit
    {
        get { return _isShowCrit; }
        set
        {
            SetProperty(ref _isShowCrit, value);

            _powerUpInCombat.UpdateProperty("IsCrit");
            _powerUpInCombat.UpdateCollection(_healDoneInformationsWithOverheal);
            DetailsInformations = _powerUpInCombat.ShowSpecificalValue("Time", DetailsInformations, value);

            RaisePropertyChanged(() => DetailsInformations);
        }
    }

    #endregion

    protected override void ChildPrepare(CombatPlayerModel parameter)
    {
        var healDoneCollection = _cacheService.GetDataFromCache<Dictionary<string, List<HealDone>>>($"{AppCacheKeys.CombatDetails_HealDone}_{SelectedCombat.LocallyNumber}");
        var healDoneCollectionMap = _mapper.Map<List<HealDoneModel>>(healDoneCollection[parameter.PlayerId]);
        DetailsInformations = new ObservableCollection<HealDoneModel>(healDoneCollectionMap);

        var healDoneGeneralCollection = _cacheService.GetDataFromCache<Dictionary<string, List<HealDoneGeneral>>>($"{AppCacheKeys.CombatDetails_HealDoneGeneral}_{SelectedCombat.LocallyNumber}");
        var healDoneGeneralCollectionMap = _mapper.Map<List<HealDoneGeneralModel>>(healDoneGeneralCollection[parameter.PlayerId]);
        GeneralInformations = new ObservableCollection<HealDoneGeneralModel>(healDoneGeneralCollectionMap);
    }

    protected override void Filter()
    {
        DetailsInformations = _healDoneInformationsWithoutFilter.Any(x => x.Spell == SelectedSource)
            ? new ObservableCollection<HealDoneModel>(_healDoneInformationsWithoutFilter.Where(x => x.Spell == SelectedSource))
            : _healDoneInformationsWithoutFilter;
    }

    protected override async Task LoadCountAsync()
    {
        var count = await _combatParserAPIService.LoadCountAsync($"HealDone/count/{SelectedPlayerId}");
        Count = count;

        var pages = (double)count / (double)_pageSize;
        var maxPages = (int)Math.Ceiling(pages);
        MaxPages = maxPages;
    }

    protected override async Task LoadDetailsAsync(int page, int pageSize)
    {
        var details = await _combatParserAPIService.LoadCombatDetailsAsync<HealDoneModel>($"HealDone/getByCombatPlayerId?combatPlayerId={SelectedPlayerId}&page={page}&pageSize={pageSize}");
        DetailsInformations = new ObservableCollection<HealDoneModel>(details.ToList());
    }

    protected override async Task LoadGenericDetailsAsync()
    {
        var generalDetails = await _combatParserAPIService.LoadCombatDetailsAsync<HealDoneGeneralModel>($"HealDoneGeneral/getByCombatPlayerId/{SelectedPlayerId}");
        GeneralInformations = new ObservableCollection<HealDoneGeneralModel>(generalDetails.ToList());
    }

    protected override void SetUpFilteredCollection()
    {
        _healDoneInformationsWithoutFilter = new ObservableCollection<HealDoneModel>(DetailsInformations);
        _healDoneInformationsWithOverheal = new ObservableCollection<HealDoneModel>(DetailsInformations);
    }

    protected override void SetTotalValue(CombatPlayerModel parameter)
    {
        TotalValue = parameter.HealDone;
    }

    protected override void TurnOnAllFilters()
    {
        if (!IsShowOverheal) IsShowOverheal = true;
        if (!IsShowCrit) IsShowCrit = true;
    }
}
