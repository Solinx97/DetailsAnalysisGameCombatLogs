using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Patterns;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class HealDoneDetailsViewModel : MvxViewModel<Tuple<CombatPlayerModel, CombatModel>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly CombatParserAPIService _combatParserAPIService;
        private readonly PowerUpInCombat<HealDoneModel> _powerUpInCombat;

        private IImprovedMvxViewModel _basicTemplate;
        private ObservableCollection<HealDoneModel> _healDoneInformations;
        private ObservableCollection<HealDoneModel> _healDoneInformationsWithoutFilter;
        private ObservableCollection<HealDoneModel> _healDoneInformationsWithOverheal;
        private ObservableCollection<string> _healDoneSources;
        private ObservableCollection<HealDoneGeneralModel> _healDoneGeneralInformations;

        private bool _isShowOverheal = true;
        private bool _isShowCrit = true;
        private bool _isShowFilters;
        private string _selectedPlayer;
        private string _selectedHealDoneSource = "Все";
        private long _totalValue;

        public HealDoneDetailsViewModel(IMapper mapper, IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _logger = logger;

            _combatParserAPIService = new CombatParserAPIService(httpClient, logger, memoryCache);
            _powerUpInCombat = new PowerUpInCombat<HealDoneModel>(_healDoneInformationsWithOverheal);

            BasicTemplate = Templates.Basic;
            BasicTemplate.Parent = this;
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 4);
        }

        public IImprovedMvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public ObservableCollection<HealDoneModel> HealDoneInformations
        {
            get { return _healDoneInformations; }
            set
            {
                SetProperty(ref _healDoneInformations, value);
            }
        }

        public ObservableCollection<string> HealDoneSources
        {
            get { return _healDoneSources; }
            set
            {
                SetProperty(ref _healDoneSources, value);
            }
        }

        public ObservableCollection<HealDoneGeneralModel> HealDoneGeneralInformations
        {
            get { return _healDoneGeneralInformations; }
            set
            {
                SetProperty(ref _healDoneGeneralInformations, value);
            }
        }

        public bool IsShowOverheal
        {
            get { return _isShowOverheal; }
            set
            {
                SetProperty(ref _isShowOverheal, value);

                _powerUpInCombat.UpdateProperty("IsFullOverheal");
                _powerUpInCombat.UpdateCollection(_healDoneInformationsWithOverheal);
                HealDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", HealDoneInformations, value);

                RaisePropertyChanged(() => HealDoneInformations);
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
                HealDoneInformations = _powerUpInCombat.ShowSpecificalValue("Time", HealDoneInformations, value);

                RaisePropertyChanged(() => HealDoneInformations);
            }
        }

        public bool IsShowFilters
        {
            get { return _isShowFilters; }
            set
            {
                SetProperty(ref _isShowFilters, value);
            }
        }

        public string SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                SetProperty(ref _selectedPlayer, value);
            }
        }

        public string SelectedHealDoneSource
        {
            get { return _selectedHealDoneSource; }
            set
            {
                SetProperty(ref _selectedHealDoneSource, value);

                HealDoneInformationFilter();
            }
        }

        public long TotalValue
        {
            get { return _totalValue; }
            set
            {
                SetProperty(ref _totalValue, value);
            }
        }

        public override void Prepare(Tuple<CombatPlayerModel, CombatModel> parameter)
        {
            var combat = parameter.Item2;
            var player = parameter.Item1;
            SelectedPlayer = player.UserName;
            TotalValue = player.HealDone;

            if (player.Id > 0)
            {
                Task.Run(async () => await LoadHealDoneDetails(player.Id));
                Task.Run(async () => await LoadHealDoneGeneral(player.Id));
            }
            else
            {
                CombatDetailsTemplate combatInformation = new CombatDetailsHealDone(_logger);
                var map = _mapper.Map<Combat>(combat);

                GetHealDoneDetails(combatInformation, SelectedPlayer, map);
                GetHealDoneGeneral(combatInformation, map);
            }
        }

        private void GetHealDoneDetails(CombatDetailsTemplate combatInformation, string player, Combat combat)
        {
            combatInformation.GetData(player, combat.Data);

            var map1 = _mapper.Map<ObservableCollection<HealDoneModel>>(combatInformation.HealDone);

            HealDoneInformations = map1;
            _healDoneInformationsWithoutFilter = new ObservableCollection<HealDoneModel>(map1);
            _healDoneInformationsWithOverheal = new ObservableCollection<HealDoneModel>(map1);

            var healDOneSources = HealDoneInformations.Select(x => x.SpellOrItem).Distinct().ToList();
            healDOneSources.Insert(0, "Все");
            HealDoneSources = new ObservableCollection<string>(healDOneSources);
        }

        private void GetHealDoneGeneral(CombatDetailsTemplate combatInformation, Combat combat)
        {
            var damageDoneGeneralInformations = combatInformation.GetHealDoneGeneral(combatInformation.HealDone, combat);
            var map2 = _mapper.Map<ObservableCollection<HealDoneGeneralModel>>(damageDoneGeneralInformations);
            HealDoneGeneralInformations = map2;
        }

        private async Task LoadHealDoneDetails(int combatPlayerId)
        {
            var healDones = await _combatParserAPIService.LoadHealDoneDetailsAsync(combatPlayerId);
            HealDoneInformations = new ObservableCollection<HealDoneModel>(healDones.ToList());
            _healDoneInformationsWithOverheal = new ObservableCollection<HealDoneModel>(healDones.ToList());
        }

        private async Task LoadHealDoneGeneral(int combatPlayerId)
        {
            var healDoneGenerals = await _combatParserAPIService.LoadHealDoneGeneralAsync(combatPlayerId);
            HealDoneGeneralInformations = new ObservableCollection<HealDoneGeneralModel>(healDoneGenerals.ToList());
        }

        private void HealDoneInformationFilter()
        {
            if (_healDoneInformationsWithoutFilter.Any(x => x.SpellOrItem == SelectedHealDoneSource))
            {
                HealDoneInformations = new ObservableCollection<HealDoneModel>(_healDoneInformationsWithoutFilter.Where(x => x.SpellOrItem == SelectedHealDoneSource));
            }
            else
            {
                HealDoneInformations = _healDoneInformationsWithoutFilter;
            }
        }
    }
}
