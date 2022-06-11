using AutoMapper;
using CombatAnalysis.CombatParser;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.Core.Commands;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CombatAnalysis.Core.ViewModels
{
    public class DamageTakenDetailsViewModel : MvxViewModel<Tuple<string, CombatModel>>
    {
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly IViewModelConnect _handler;
        private readonly IMapper _mapper;
        private readonly PowerUpInCombat<DamageTakenModel> _powerUpInCombat;

        private MvxViewModel _basicTemplate;
        private ObservableCollection<DamageTakenModel> _damageTakenInformations;
        private ObservableCollection<DamageTakenModel> _damageTakenInformationsWithSkipDamage;
        private ObservableCollection<DamageTakenGeneralModel> _damageTakenGeneralInformations;

        private bool _isShowDodge = true;
        private bool _isShowParry = true;
        private bool _isShowMiss = true;
        private bool _isShowResist = true;
        private bool _isShowImmune = true;
        private string _selectedPlayer;
        private int _selectedIndexSorting;
        private bool _isCollectionReversed;
        private long _totalValue;

        public DamageTakenDetailsViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation)
        {
            _mapper = mapper;
            _mvvmNavigation = mvvmNavigation;

            _handler = new ViewModelMConnect();
            BasicTemplate = new BasicTemplateViewModel(this, _handler, _mvvmNavigation);

            _handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 5);

            _powerUpInCombat = new PowerUpInCombat<DamageTakenModel>(_damageTakenInformationsWithSkipDamage);
        }

        public MvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public ObservableCollection<DamageTakenModel> DamageTakenInformations
        {
            get { return _damageTakenInformations; }
            set
            {
                SetProperty(ref _damageTakenInformations, value);
            }
        }

        public ObservableCollection<DamageTakenGeneralModel> DamageTakenGeneralInformations
        {
            get { return _damageTakenGeneralInformations; }
            set
            {
                SetProperty(ref _damageTakenGeneralInformations, value);
            }
        }

        public bool IsShowDodge
        {
            get { return _isShowDodge; }
            set
            {
                SetProperty(ref _isShowDodge, value);

                _powerUpInCombat.UpdateProperty("IsDodge");
                _powerUpInCombat.UpdateCollection(_damageTakenInformationsWithSkipDamage);
                DamageTakenInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageTakenInformations, value);

                RaisePropertyChanged(() => DamageTakenInformations);
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
                DamageTakenInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageTakenInformations, value);

                RaisePropertyChanged(() => DamageTakenInformations);
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
                DamageTakenInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageTakenInformations, value);

                RaisePropertyChanged(() => DamageTakenInformations);
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
                DamageTakenInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageTakenInformations, value);

                RaisePropertyChanged(() => DamageTakenInformations);
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
                DamageTakenInformations = _powerUpInCombat.ShowSpecificalValue("Time", DamageTakenInformations, value);

                RaisePropertyChanged(() => DamageTakenInformations);
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

                RaisePropertyChanged(() => DamageTakenGeneralInformations);
            }
        }

        public bool IsCollectionReversed
        {
            get { return _isCollectionReversed; }
            set
            {
                SetProperty(ref _isCollectionReversed, value);

                Reverse();

                RaisePropertyChanged(() => DamageTakenGeneralInformations);
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

        public override void Prepare(Tuple<string, CombatModel> parameter)
        {
            TotalValue = parameter.Item2.Players.Find(x => x.UserName == parameter.Item1).DamageTaken;

            GetHealDoneDetails(parameter);
        }

        private void GetHealDoneDetails(Tuple<string, CombatModel> combatInformationData)
        {
            var combatInformation = new CombatInformation();

            var map = _mapper.Map<Combat>(combatInformationData.Item2);
            combatInformation.SetCombat(map, combatInformationData.Item1);
            combatInformation.GetDamageTaken();

            var map1 = _mapper.Map<ObservableCollection<DamageTakenModel>>(combatInformation.DamageTakenInformations);

            DamageTakenInformations = map1;
            _damageTakenInformationsWithSkipDamage = new ObservableCollection<DamageTakenModel>(map1);

            var damageDoneGeneralInformations = combatInformation.GetDamageTakenGeneral(combatInformation.DamageTakenInformations, map);
            var map2 = _mapper.Map<ObservableCollection<DamageTakenGeneralModel>>(damageDoneGeneralInformations);
            DamageTakenGeneralInformations = map2;
        }

        private void Sorting(int index)
        {
            IOrderedEnumerable<DamageTakenGeneralModel> sortedCollection;

            switch (index)
            {
                case 0:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.SpellOrItem);
                    break;
                case 1:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.Value);
                    break;
                case 2:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.CastNumber);
                    break;
                case 3:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.MinValue);
                    break;
                case 4:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.MaxValue);
                    break;
                case 5:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.AverageValue);
                    break;
                case 6:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.DamageTakenPerSecond);
                    break;
                default:
                    sortedCollection = DamageTakenGeneralInformations.OrderBy(x => x.Value);
                    break;
            }

            DamageTakenGeneralInformations = new ObservableCollection<DamageTakenGeneralModel>(sortedCollection.ToList());
            IsCollectionReversed = false;
        }

        private void Reverse()
        {
            DamageTakenGeneralInformations = new ObservableCollection<DamageTakenGeneralModel>(DamageTakenGeneralInformations.Reverse().ToList());
        }
    }
}
