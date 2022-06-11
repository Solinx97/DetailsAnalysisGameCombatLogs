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

        public override void Prepare(Tuple<string, CombatModel> parameter)
        {
            SelectedPlayer = parameter.Item1;

            GetResourceRecoveryDetails(parameter);
        }

        private void GetResourceRecoveryDetails(Tuple<string, CombatModel> combatInformationData)
        {
            var combatInformation = new CombatInformation();

            var map = _mapper.Map<Combat>(combatInformationData.Item2);
            combatInformation.SetCombat(map, combatInformationData.Item1);
            combatInformation.GetEnergyRecovery();

            var map1 = _mapper.Map<ObservableCollection<ResourceRecoveryModel>>(combatInformation.ResourceRecoveryInformations);

            ResourceRecoveryInformations = map1;
            _resourceRecoveryInformations = new ObservableCollection<ResourceRecoveryModel>(map1);

            var damageDoneGeneralInformations = combatInformation.GetResourceRecoveryGeneral(combatInformation.ResourceRecoveryInformations, map);
            var map2 = _mapper.Map<ObservableCollection<ResourceRecoveryGeneralModel>>(damageDoneGeneralInformations);
            ResourceRecoveryGeneralInformations = map2;
        }
    }
}
