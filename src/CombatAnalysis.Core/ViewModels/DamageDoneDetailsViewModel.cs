using AutoMapper;
using CombatAnalysis.CombatParser;
using CombatAnalysis.CombatParser.Models;
using CombatAnalysis.Core.Commands;
using CombatAnalysis.Core.Core;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CombatAnalysis.Core.ViewModels
{
    public class DamageDoneDetailsViewModel : MvxViewModel<Tuple<string, CombatModel>>
    {
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly IViewModelConnect _handler;
        private readonly IMapper _mapper;
        private readonly PowerUpInCombat<DamageDoneModel> _powerUpInCombat;

        private MvxViewModel _basicTemplate;
        private ObservableCollection<DamageDoneModel> _damageDoneInformations;
        private ObservableCollection<DamageDoneModel> _damageDoneInformationsWithSkipDamage;
        private ObservableCollection<DamageDoneModel> _damageDoneGroupBySpellOrItem;
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
        private int _selectedDetailsType;

        public DamageDoneDetailsViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation)
        {
            _mvvmNavigation = mvvmNavigation;
            _mapper = mapper;

            _handler = new ViewModelMConnect();
            BasicTemplate = new BasicTemplateViewModel(this, _handler, _mvvmNavigation);

            _handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 3);

            _powerUpInCombat = new PowerUpInCombat<DamageDoneModel>(_damageDoneInformationsWithSkipDamage);
        }

        public MvxViewModel BasicTemplate
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

        public int SelectedDetailsType
        {
            get { return _selectedDetailsType; }
            set
            {
                SetProperty(ref _selectedDetailsType, value);

                SwitchBetweenDetailsType(value);

                RaisePropertyChanged(() => DamageDoneInformations);
            }
        }

        public override void Prepare(Tuple<string, CombatModel> parameter)
        {
            GetDamageDoneDetails(parameter);
            GroupBySpellOrItem();
        }

        private void GetDamageDoneDetails(Tuple<string, CombatModel> combatInformationData)
        {
            var combatInformation = new CombatInformation();

            var map = _mapper.Map<Combat>(combatInformationData.Item2);
            combatInformation.SetCombat(map, combatInformationData.Item1);
            combatInformation.GetDamageDone();

            var map1 = _mapper.Map<ObservableCollection<DamageDoneModel>>(combatInformation.DamageDoneInformations);

            DamageDoneInformations = map1;
            _damageDoneInformationsWithSkipDamage = new ObservableCollection<DamageDoneModel>(map1);
        }

        private void SwitchBetweenDetailsType(int type)
        {
            switch (type)
            {
                case 0:
                    DamageDoneInformations = new ObservableCollection<DamageDoneModel>(_damageDoneInformationsWithSkipDamage);
                    break;
                case 1:
                    DamageDoneInformations = new ObservableCollection<DamageDoneModel>(_damageDoneGroupBySpellOrItem);
                    break;
                default:
                    break;
            }
        }

        private void GroupBySpellOrItem()
        {
            var spells = DamageDoneInformations
                .GroupBy(group => group.SpellOrItem)
                .Select(select => select.ToList())
                .ToList();

            var lessDetails = new List<DamageDoneModel>();
            foreach (var item in spells)
            {
                var damageDone = new DamageDoneModel
                {
                    Value = item.Sum(x => x.Value),
                    SpellOrItem = item[0].SpellOrItem,
                };

                lessDetails.Add(damageDone);
            }

            lessDetails = lessDetails.OrderByDescending(x => x.Value).ToList();

            _damageDoneGroupBySpellOrItem = new ObservableCollection<DamageDoneModel>(lessDetails);
        }
    }
}