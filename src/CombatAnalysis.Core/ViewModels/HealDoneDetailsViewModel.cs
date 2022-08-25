using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Services;
using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.Core.Services;
using Microsoft.Extensions.Logging;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class HealDoneDetailsViewModel : MvxViewModel<Tuple<int, CombatModel>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly CombatParserAPIService _combatParserAPIService;
        private readonly PowerUpInCombat<HealDoneModel> _powerUpInCombat;

        private IImprovedMvxViewModel _basicTemplate;
        private ObservableCollection<HealDoneModel> _healDoneInformations;
        private ObservableCollection<HealDoneModel> _healDoneInformationsWithOverheal;
        private ObservableCollection<HealDoneGeneralModel> _healDoneGeneralInformations;

        private bool _isShowOverheal = true;
        private bool _isShowCrit = true;
        private bool _isShowOnlyOverheal;
        private bool _isShowOnlyCrit;
        private string _selectedPlayer;
        private int _selectedIndexSorting;
        private bool _isCollectionReversed;
        private long _totalValue;

        public HealDoneDetailsViewModel(IMapper mapper, IHttpClientHelper httpClient, ILogger logger)
        {
            _mapper = mapper;
            _logger = logger;

            _combatParserAPIService = new CombatParserAPIService(httpClient);
            _powerUpInCombat = new PowerUpInCombat<HealDoneModel>(_healDoneInformationsWithOverheal);

            BasicTemplate = Templates.Basic;
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

        public bool IsShowOnlyOverheal
        {
            get { return _isShowOnlyOverheal; }
            set
            {
                SetProperty(ref _isShowOnlyOverheal, value);

                _powerUpInCombat.UpdateProperty("IsFullOverheal");
                _powerUpInCombat.UpdateCollection(_healDoneInformationsWithOverheal);
                HealDoneInformations = _powerUpInCombat.ShowSpecificalValueInversion("Time", HealDoneInformations, value);

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

        public bool IsShowOnlyCrit
        {
            get { return _isShowOnlyCrit; }
            set
            {
                SetProperty(ref _isShowOnlyCrit, value);

                _powerUpInCombat.UpdateProperty("IsCrit");
                _powerUpInCombat.UpdateCollection(_healDoneInformationsWithOverheal);
                HealDoneInformations = _powerUpInCombat.ShowSpecificalValueInversion("Time", HealDoneInformations, value);

                RaisePropertyChanged(() => HealDoneInformations);
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

        public int SelectedIndexSorting
        {
            get { return _selectedIndexSorting; }
            set
            {
                SetProperty(ref _selectedIndexSorting, value);

                Sorting(value);

                RaisePropertyChanged(() => HealDoneGeneralInformations);
            }
        }

        public bool IsCollectionReversed
        {
            get { return _isCollectionReversed; }
            set
            {
                SetProperty(ref _isCollectionReversed, value);

                Reverse();

                RaisePropertyChanged(() => HealDoneGeneralInformations);
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

        public override void Prepare(Tuple<int, CombatModel> parameter)
        {
            var combat = parameter.Item2;
            var player = combat.Players[parameter.Item1];
            SelectedPlayer = player.UserName;
            TotalValue = player.HealDone;

            if (player.Id > 0)
            {
                Task.Run(async () => await LoadHealDoneDetails(player.Id));
                Task.Run(async () => await LoadHealDoneGeneral(player.Id));
            }
            else
            {
                var combatInformation = new CombatDetailsService(_logger);
                var map = _mapper.Map<Combat>(combat);

                GetHealDoneDetails(combatInformation, SelectedPlayer, map);
                GetHealDoneGeneral(combatInformation, map);
            }
        }

        private void GetHealDoneDetails(CombatDetailsService combatInformation, string player, Combat combat)
        {
            combatInformation.Initialization(combat, player);
            combatInformation.GetHealDone();

            var map1 = _mapper.Map<ObservableCollection<HealDoneModel>>(combatInformation.HealDone);

            HealDoneInformations = map1;
            _healDoneInformationsWithOverheal = new ObservableCollection<HealDoneModel>(map1);
        }

        private void GetHealDoneGeneral(CombatDetailsService combatInformation, Combat combat)
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

        private void Sorting(int index)
        {
            IOrderedEnumerable<HealDoneGeneralModel> sortedCollection;

            switch (index)
            {
                case 0:
                    sortedCollection = HealDoneGeneralInformations.OrderBy(x => x.SpellOrItem);
                    break;
                case 1:
                    sortedCollection = HealDoneGeneralInformations.OrderBy(x => x.Value);
                    break;
                case 2:
                    sortedCollection = HealDoneGeneralInformations.OrderBy(x => x.CastNumber);
                    break;
                case 3:
                    sortedCollection = HealDoneGeneralInformations.OrderBy(x => x.MinValue);
                    break;
                case 4:
                    sortedCollection = HealDoneGeneralInformations.OrderBy(x => x.MaxValue);
                    break;
                case 5:
                    sortedCollection = HealDoneGeneralInformations.OrderBy(x => x.AverageValue);
                    break;
                case 6:
                    sortedCollection = HealDoneGeneralInformations.OrderBy(x => x.HealPerSecond);
                    break;
                default:
                    sortedCollection = HealDoneGeneralInformations.OrderBy(x => x.Value);
                    break;
            }

            HealDoneGeneralInformations = new ObservableCollection<HealDoneGeneralModel>(sortedCollection.ToList());
            IsCollectionReversed = false;
        }

        private void Reverse()
        {
            HealDoneGeneralInformations = new ObservableCollection<HealDoneGeneralModel>(HealDoneGeneralInformations.Reverse().ToList());
        }
    }
}
