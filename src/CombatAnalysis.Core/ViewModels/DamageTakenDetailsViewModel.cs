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
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels
{
    public class DamageTakenDetailsViewModel : MvxViewModel<Tuple<string, CombatModel>>
    {
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly IViewModelConnect _handler;
        private readonly IMapper _mapper;

        private MvxViewModel _basicTemplate;
        private ObservableCollection<DamageTakenInformationModel> _damageTakenInformations;
        private ObservableCollection<DamageTakenInformationModel> _damageTakenInformationsWithSkipDamage;
        private bool _isShowDodge = true;
        private bool _isShowParry = true;
        private bool _isShowMiss = true;
        private bool _isShowResist = true;
        private bool _isShowImmune = true;

        public DamageTakenDetailsViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation)
        {
            _mapper = mapper;
            _mvvmNavigation = mvvmNavigation;

            _handler = new ViewModelMConnect();
            BasicTemplate = new BasicTemplateViewModel(this, _handler, _mvvmNavigation);

            _handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 5);
        }

        public MvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public ObservableCollection<DamageTakenInformationModel> DamageTakenInformations
        {
            get { return _damageTakenInformations; }
            set
            {
                SetProperty(ref _damageTakenInformations, value);
            }
        }

        public bool IsShowDodge
        {
            get { return _isShowDodge; }
            set
            {
                SetProperty(ref _isShowDodge, value);
                ShowDodge(value);

                RaisePropertyChanged(() => DamageTakenInformations);
            }
        }

        public bool IsShowParry
        {
            get { return _isShowParry; }
            set
            {
                SetProperty(ref _isShowParry, value);
                ShowParry(value);

                RaisePropertyChanged(() => DamageTakenInformations);
            }
        }

        public bool IsShowMiss
        {
            get { return _isShowMiss; }
            set
            {
                SetProperty(ref _isShowMiss, value);
                ShowMiss(value);

                RaisePropertyChanged(() => DamageTakenInformations);
            }
        }

        public bool IsShowResist
        {
            get { return _isShowResist; }
            set
            {
                SetProperty(ref _isShowResist, value);
                ShowResist(value);

                RaisePropertyChanged(() => DamageTakenInformations);
            }
        }

        public bool IsShowImmune
        {
            get { return _isShowImmune; }
            set
            {
                SetProperty(ref _isShowImmune, value);
                ShowImmune(value);

                RaisePropertyChanged(() => DamageTakenInformations);
            }
        }

        public override void Prepare(Tuple<string, CombatModel> parameter)
        {
            GetHealDoneDetails(parameter);
        }

        private void GetHealDoneDetails(Tuple<string, CombatModel> combatInformationData)
        {
            var combatInformation = new CombatInformation();

            var map = _mapper.Map<Combat>(combatInformationData.Item2);
            combatInformation.SetCombat(map, combatInformationData.Item1);
            combatInformation.GetDamageTaken();

            var map1 = _mapper.Map<ObservableCollection<DamageTakenInformationModel>>(combatInformation.DamageTakenInformations);

            DamageTakenInformations = map1;
            _damageTakenInformationsWithSkipDamage = new ObservableCollection<DamageTakenInformationModel>(map1);
        }

        private void ShowDodge(bool isShowDodge)
        {
            if (!isShowDodge)
            {
                foreach (var item in _damageTakenInformationsWithSkipDamage)
                {
                    if (item.IsDodge)
                    {
                        DamageTakenInformations.Remove(item);
                    }
                }
            }
            else
            {
                foreach (var item in _damageTakenInformationsWithSkipDamage)
                {
                    if (item.IsDodge)
                    {
                        DamageTakenInformations.Add(item);
                    }
                }

                DamageTakenInformations = Sorts<DamageTakenInformationModel>.BubbleSort(DamageTakenInformations);
            }
        }

        private void ShowParry(bool isShowParry)
        {
            if (!isShowParry)
            {
                foreach (var item in _damageTakenInformationsWithSkipDamage)
                {
                    if (item.IsParry)
                    {
                        DamageTakenInformations.Remove(item);
                    }
                }
            }
            else
            {
                foreach (var item in _damageTakenInformationsWithSkipDamage)
                {
                    if (item.IsParry)
                    {
                        DamageTakenInformations.Add(item);
                    }
                }

                DamageTakenInformations = Sorts<DamageTakenInformationModel>.BubbleSort(DamageTakenInformations);
            }
        }

        private void ShowMiss(bool isShowMiss)
        {
            if (!isShowMiss)
            {
                foreach (var item in _damageTakenInformationsWithSkipDamage)
                {
                    if (item.IsMiss)
                    {
                        DamageTakenInformations.Remove(item);
                    }
                }
            }
            else
            {
                foreach (var item in _damageTakenInformationsWithSkipDamage)
                {
                    if (item.IsMiss)
                    {
                        DamageTakenInformations.Add(item);
                    }
                }

                DamageTakenInformations = Sorts<DamageTakenInformationModel>.BubbleSort(DamageTakenInformations);
            }
        }

        private void ShowResist(bool isShowResist)
        {
            if (!isShowResist)
            {
                foreach (var item in _damageTakenInformationsWithSkipDamage)
                {
                    if (item.IsResist)
                    {
                        DamageTakenInformations.Remove(item);
                    }
                }
            }
            else
            {
                foreach (var item in _damageTakenInformationsWithSkipDamage)
                {
                    if (item.IsResist)
                    {
                        DamageTakenInformations.Add(item);
                    }
                }

                DamageTakenInformations = Sorts<DamageTakenInformationModel>.BubbleSort(DamageTakenInformations);
            }
        }

        private void ShowImmune(bool isShowImmune)
        {
            if (!isShowImmune)
            {
                foreach (var item in _damageTakenInformationsWithSkipDamage)
                {
                    if (item.IsImmune)
                    {
                        DamageTakenInformations.Remove(item);
                    }
                }
            }
            else
            {
                foreach (var item in _damageTakenInformationsWithSkipDamage)
                {
                    if (item.IsImmune)
                    {
                        DamageTakenInformations.Add(item);
                    }
                }

                DamageTakenInformations = Sorts<DamageTakenInformationModel>.BubbleSort(DamageTakenInformations);
            }
        }
    }
}
