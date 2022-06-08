using AutoMapper;
using CombatAnalysis.CombatParser;
using CombatAnalysis.Core.Commands;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class GeneralAnalysisViewModel : MvxViewModel<string>
    {
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly IMapper _mapper;
        private readonly IViewModelConnect _handler;

        private int _combatIndex;
        private MvxViewModel _basicTemplate;
        private List<CombatModel> _combats;

        public GeneralAnalysisViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation)
        {
            _mapper = mapper;
            _mvvmNavigation = mvvmNavigation;

            _combats = new List<CombatModel>();

            ShowDetailsCommand = new MvxCommand(ShowDetails);

            _handler = new ViewModelMConnect();
            BasicTemplate = new BasicTemplateViewModel(this, _handler, mvvmNavigation);

            _handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 1);
        }

        public IMvxCommand ShowDetailsCommand { get; set; }

        public MvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public List<CombatModel> Combats
        {
            get { return _combats; }
            set
            {
                SetProperty(ref _combats, value);
            }
        }

        public int CombatIndex
        {
            get { return _combatIndex; }
            set
            {
                SetProperty(ref _combatIndex, value);
            }
        }

        public override void Prepare(string parameter)
        {
            var task = Task.Run(() => GetData(parameter));
            task.Wait();
        }

        public void ShowDetails()
        {
            Task.Run(() => _mvvmNavigation.Navigate<TargetCombatDetailsViewModel, CombatModel>(Combats[CombatIndex]));
        }

        private async Task GetData(string combatLog)
        {
            var parser = new CombatInformationParser();
            await parser.Parse(combatLog);

            var combats = parser.GetCombats();
            var combatsMapper = _mapper.Map<List<CombatModel>>(combats);
            Combats.AddRange(combatsMapper);

            GetDetails();
        }

        private void GetDetails()
        {
            for (int i = 0; i < Combats.Count; i++)
            {
                foreach (var item in Combats[i].Players)
                {
                    Combats[i].DamageDone += item.DamageDone;
                    Combats[i].HealDone += item.HealDone;
                    Combats[i].EnergyRecovery += item.EnergyRecovery;
                    Combats[i].DamageTaken += item.DamageTaken;
                }
            }
        }
    }
}
