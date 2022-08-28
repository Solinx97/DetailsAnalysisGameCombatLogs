using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Interfaces;
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
    public class DamageDoneDetailsViewModel : MvxViewModel<Tuple<int, CombatModel>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly PowerUpInCombat<DamageDoneModel> _powerUpInCombat;
        private readonly CombatParserAPIService _combatParserAPIService;

        private IImprovedMvxViewModel _basicTemplate;
        private ObservableCollection<DamageDoneModel> _damageDoneInformations;
        private ObservableCollection<DamageDoneModel> _damageDoneInformationsWithSkipDamage;
        private ObservableCollection<DamageDoneGeneralModel> _damageDoneGeneralInformations;

        private bool _isShowCrit = true;
        private bool _isShowDodge = true;
        private bool _isShowParry = true;
        private bool _isShowMiss = true;
        private bool _isShowResist = true;
        private bool _isShowImmune = true;
        private bool _isShowOnlyCrit;
        private bool _isShowOnlyDodge;
        private bool _isShowOnlyParry;
        private bool _isShowOnlyMiss;
        private bool _isShowOnlyResist;
        private bool _isShowOnlyImmune;
        private string _selectedPlayer;
        private int _selectedIndexSorting;
        private bool _isCollectionReversed;
        private long _totalValue;

        public DamageDoneDetailsViewModel(IMapper mapper, IHttpClientHelper httpClient, ILogger loger)
        {
            _mapper = mapper;
            _logger = loger;

            _combatParserAPIService = new CombatParserAPIService(httpClient, loger);
            _powerUpInCombat = new PowerUpInCombat<DamageDoneModel>(_damageDoneInformationsWithSkipDamage);

            BasicTemplate = Templates.Basic;
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 3);
        }

        public IImprovedMvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public ObservableCollection<DamageDoneModel> DamageDoneInformations
        {
            get { return _damageDoneInformations; }
            set
            {
                SetProperty(ref _damageDoneInformations, value);
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

        public bool IsShowOnlyCrit
        {
            get { return _isShowOnlyCrit; }
            set
            {
                SetProperty(ref _isShowOnlyCrit, value);

                _powerUpInCombat.UpdateProperty("IsCrit");
                _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
                DamageDoneInformations = _powerUpInCombat.ShowSpecificalValueInversion("Time", DamageDoneInformations, value);

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

        public bool IsShowOnlyDodge
        {
            get { return _isShowOnlyDodge; }
            set
            {
                SetProperty(ref _isShowOnlyDodge, value);

                _powerUpInCombat.UpdateProperty("IsDodge");
                _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
                DamageDoneInformations = _powerUpInCombat.ShowSpecificalValueInversion("Time", DamageDoneInformations, value);

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

        public bool IsShowOnlyParry
        {
            get { return _isShowOnlyParry; }
            set
            {
                SetProperty(ref _isShowOnlyParry, value);

                _powerUpInCombat.UpdateProperty("IsParry");
                _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
                DamageDoneInformations = _powerUpInCombat.ShowSpecificalValueInversion("Time", DamageDoneInformations, value);

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

        public bool IsShowOnlyMiss
        {
            get { return _isShowOnlyMiss; }
            set
            {
                SetProperty(ref _isShowOnlyMiss, value);

                _powerUpInCombat.UpdateProperty("IsMiss");
                _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
                DamageDoneInformations = _powerUpInCombat.ShowSpecificalValueInversion("Time", DamageDoneInformations, value);

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

        public bool IsShowOnlyResist
        {
            get { return _isShowOnlyResist; }
            set
            {
                SetProperty(ref _isShowOnlyResist, value);

                _powerUpInCombat.UpdateProperty("IsResist");
                _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
                DamageDoneInformations = _powerUpInCombat.ShowSpecificalValueInversion("Time", DamageDoneInformations, value);

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

        public bool IsShowOnlyImmune
        {
            get { return _isShowOnlyImmune; }
            set
            {
                SetProperty(ref _isShowOnlyImmune, value);

                _powerUpInCombat.UpdateProperty("IsImmune");
                _powerUpInCombat.UpdateCollection(_damageDoneInformationsWithSkipDamage);
                DamageDoneInformations = _powerUpInCombat.ShowSpecificalValueInversion("Time", DamageDoneInformations, value);

                RaisePropertyChanged(() => DamageDoneInformations);
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

                RaisePropertyChanged(() => DamageDoneGeneralInformations);
            }
        }

        public bool IsCollectionReversed
        {
            get { return _isCollectionReversed; }
            set
            {
                SetProperty(ref _isCollectionReversed, value);

                Reverse();

                RaisePropertyChanged(() => DamageDoneGeneralInformations);
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
            TotalValue = player.DamageDone;

            if (player.Id > 0)
            {
                Task.Run(async () => await LoadDamageDoneDetails(player.Id));
                Task.Run(async () => await LoadDamageDoneGeneral(player.Id));
            }
            else
            {
                var combatInformation = new CombatDetailsService(_logger);
                var map = _mapper.Map<Combat>(combat);

                GetDamageDoneDetails(combatInformation, SelectedPlayer, map);
                GetDamageDoneGeneral(combatInformation, map);
            }
        }

        private void GetDamageDoneDetails(ICombatDetails combatDetails, string player, Combat combat)
        {
            combatDetails.Initialization(combat, player);
            combatDetails.GetDamageDone();

            var map1 = _mapper.Map<ObservableCollection<DamageDoneModel>>(combatDetails.DamageDone);

            DamageDoneInformations = map1;
            _damageDoneInformationsWithSkipDamage = new ObservableCollection<DamageDoneModel>(map1);
        }

        private void GetDamageDoneGeneral(ICombatDetails combatDetails, Combat combat)
        {
            var damageDoneGeneralInformations = combatDetails.GetDamageDoneGeneral(combatDetails.DamageDone, combat);
            var map2 = _mapper.Map<ObservableCollection<DamageDoneGeneralModel>>(damageDoneGeneralInformations);
            DamageDoneGeneralInformations = map2;
        }

        private async Task LoadDamageDoneDetails(int combatPlayerId)
        {
            var healDones = await _combatParserAPIService.LoadDamageDoneDetailsAsync(combatPlayerId);
            DamageDoneInformations = new ObservableCollection<DamageDoneModel>(healDones.ToList());
            _damageDoneInformationsWithSkipDamage = new ObservableCollection<DamageDoneModel>(healDones.ToList());
        }

        private async Task LoadDamageDoneGeneral(int combatPlayerId)
        {
            var healDoneGenerals = await _combatParserAPIService.LoadDamageDoneGeneralAsync(combatPlayerId);
            DamageDoneGeneralInformations = new ObservableCollection<DamageDoneGeneralModel>(healDoneGenerals.ToList());
        }

        private void Sorting(int index)
        {
            IOrderedEnumerable<DamageDoneGeneralModel> sortedCollection;

            switch (index)
            {
                case 0:
                    sortedCollection = DamageDoneGeneralInformations.OrderBy(x => x.SpellOrItem);
                    break;
                case 1:
                    sortedCollection = DamageDoneGeneralInformations.OrderBy(x => x.Value);
                    break;
                case 2:
                    sortedCollection = DamageDoneGeneralInformations.OrderBy(x => x.CastNumber);
                    break;
                case 3:
                    sortedCollection = DamageDoneGeneralInformations.OrderBy(x => x.MinValue);
                    break;
                case 4:
                    sortedCollection = DamageDoneGeneralInformations.OrderBy(x => x.MaxValue);
                    break;
                case 5:
                    sortedCollection = DamageDoneGeneralInformations.OrderBy(x => x.AverageValue);
                    break;
                case 6: 
                    sortedCollection = DamageDoneGeneralInformations.OrderBy(x => x.DamagePerSecond);
                    break;
                default:
                    sortedCollection = DamageDoneGeneralInformations.OrderBy(x => x.Value);
                    break;
            }

            DamageDoneGeneralInformations = new ObservableCollection<DamageDoneGeneralModel>(sortedCollection.ToList());
            IsCollectionReversed = false;
        }

        private void Reverse()
        {
            DamageDoneGeneralInformations = new ObservableCollection<DamageDoneGeneralModel>(DamageDoneGeneralInformations.Reverse().ToList());
        }
    }
}