using AutoMapper;
using CombatAnalysis.CombatParser;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.Core.Commands;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CombatAnalysis.Core.ViewModels
{
    public class ResourceRecoveryDetailsViewModel : MvxViewModel<Tuple<string, CombatModel>>
    {
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly IViewModelConnect _handler;
        private readonly IMapper _mapper;

        private MvxViewModel _basicTemplate;
        private ObservableCollection<ResourceRecoveryModel> _resourceRecoveryInformations;
        private ObservableCollection<ResourceRecoveryGeneralModel> _resourceRecoveryGeneralInformations;

        private string _selectedPlayer;
        private int _selectedIndexSorting;
        private bool _isCollectionReversed;
        private double _totalValue;

        public ResourceRecoveryDetailsViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation)
        {
            _mapper = mapper;
            _mvvmNavigation = mvvmNavigation;

            _handler = new ViewModelMConnect();
            BasicTemplate = new BasicTemplateViewModel(this, _handler, _mvvmNavigation);

            _handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 6);
        }

        public MvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public ObservableCollection<ResourceRecoveryModel> ResourceRecoveryInformations
        {
            get { return _resourceRecoveryInformations; }
            set
            {
                SetProperty(ref _resourceRecoveryInformations, value);
            }
        }

        public ObservableCollection<ResourceRecoveryGeneralModel> ResourceRecoveryGeneralInformations
        {
            get { return _resourceRecoveryGeneralInformations; }
            set
            {
                SetProperty(ref _resourceRecoveryGeneralInformations, value);
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

                RaisePropertyChanged(() => ResourceRecoveryGeneralInformations);
            }
        }

        public bool IsCollectionReversed
        {
            get { return _isCollectionReversed; }
            set
            {
                SetProperty(ref _isCollectionReversed, value);

                Reverse();

                RaisePropertyChanged(() => ResourceRecoveryGeneralInformations);
            }
        }

        public double TotalValue
        {
            get { return _totalValue; }
            set
            {
                SetProperty(ref _totalValue, value);
            }
        }

        public override void Prepare(Tuple<string, CombatModel> parameter)
        {
            SelectedPlayer = parameter.Item1;
            TotalValue = parameter.Item2.Players.Find(x => x.UserName == parameter.Item1).EnergyRecovery;

            GetResourceRecoveryDetails(parameter);
        }

        private void GetResourceRecoveryDetails(Tuple<string, CombatModel> combatInformationData)
        {
            var combatInformation = new CombatDetailsInformation();

            var map = _mapper.Map<Combat>(combatInformationData.Item2);
            combatInformation.SetData(map, combatInformationData.Item1);
            combatInformation.GetResourceRecovery();

            var map1 = _mapper.Map<ObservableCollection<ResourceRecoveryModel>>(combatInformation.ResourceRecovery);

            ResourceRecoveryInformations = map1;
            _resourceRecoveryInformations = new ObservableCollection<ResourceRecoveryModel>(map1);

            var damageDoneGeneralInformations = combatInformation.GetResourceRecoveryGeneral(combatInformation.ResourceRecovery, map);
            var map2 = _mapper.Map<ObservableCollection<ResourceRecoveryGeneralModel>>(damageDoneGeneralInformations);
            ResourceRecoveryGeneralInformations = map2;
        }

        private void Sorting(int index)
        {
            IOrderedEnumerable<ResourceRecoveryGeneralModel> sortedCollection;

            switch (index)
            {
                case 0:
                    sortedCollection = ResourceRecoveryGeneralInformations.OrderBy(x => x.SpellOrItem);
                    break;
                case 1:
                    sortedCollection = ResourceRecoveryGeneralInformations.OrderBy(x => x.Value);
                    break;
                case 2:
                    sortedCollection = ResourceRecoveryGeneralInformations.OrderBy(x => x.CastNumber);
                    break;
                case 3:
                    sortedCollection = ResourceRecoveryGeneralInformations.OrderBy(x => x.MinValue);
                    break;
                case 4:
                    sortedCollection = ResourceRecoveryGeneralInformations.OrderBy(x => x.MaxValue);
                    break;
                case 5:
                    sortedCollection = ResourceRecoveryGeneralInformations.OrderBy(x => x.AverageValue);
                    break;
                case 6:
                    sortedCollection = ResourceRecoveryGeneralInformations.OrderBy(x => x.ResourcePerSecond);
                    break;
                default:
                    sortedCollection = ResourceRecoveryGeneralInformations.OrderBy(x => x.Value);
                    break;
            }

            ResourceRecoveryGeneralInformations = new ObservableCollection<ResourceRecoveryGeneralModel>(sortedCollection.ToList());
            IsCollectionReversed = false;
        }

        private void Reverse()
        {
            ResourceRecoveryGeneralInformations = new ObservableCollection<ResourceRecoveryGeneralModel>(ResourceRecoveryGeneralInformations.Reverse().ToList());
        }
    }
}
