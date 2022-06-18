using AutoMapper;
using CombatAnalysis.CombatParser;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.Core.Commands;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using CombatAnalysis.WinCore;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels
{
    public class MainInformationViewModel : MvxViewModel, IObserver
    {
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly IMapper _mapper;
        private readonly IHttpClientHelper _httpClient;

        private string _combatLog;
        private bool _isParsing;
        private bool _isSaving;
        private bool _isNeedSave;
        private bool _isShowSteps;
        private string _foundCombat;
        private string _combatLogPath;
        private MvxViewModel _basicTemplate;
        private IViewModelConnect _handler;
        private List<CombatModel> _combats;

        public MainInformationViewModel(IMapper mapper, IMvxNavigationService mvvmNavigation, IHttpClientHelper httpClient)
        {
            _mapper = mapper;
            _mvvmNavigation = mvvmNavigation;
            _httpClient = httpClient;

            IsNeedSave = true;

            GetCombatLogCommand = new MvxCommand(GetCombatLog);
            OpenPlayerAnalysisCommand = new MvxCommand(OpenPlayerAnalysis);

            _combats = new List<CombatModel>();

            _handler = new ViewModelMConnect();
            BasicTemplate = new BasicTemplateViewModel(this, _handler, _mvvmNavigation);
        }

        public IMvxCommand GetCombatLogCommand { get; set; }

        public IMvxCommand OpenPlayerAnalysisCommand { get; set; }

        public MvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public string CombatLog
        {
            get { return _combatLog; }
            set
            {
                SetProperty(ref _combatLog, value);
            }
        }

        public bool IsParsing
        {
            get { return _isParsing; }
            set
            {
                SetProperty(ref _isParsing, value);
            }
        }

        public bool IsSaving
        {
            get { return _isSaving; }
            set
            {
                SetProperty(ref _isSaving, value);
            }
        }

        public bool IsNeedSave
        {
            get { return _isNeedSave; }
            set
            {
                SetProperty(ref _isNeedSave, value);
            }
        }

        public bool IsShowSteps
        {
            get { return _isShowSteps; }
            set
            {
                SetProperty(ref _isShowSteps, value);
            }
        }

        public string FoundCombat
        {
            get { return _foundCombat; }
            set
            {
                SetProperty(ref _foundCombat, value);
            }
        }

        public void GetCombatLog()
        {
            _combatLogPath = WinHandler.FileOpen();
            var split = _combatLogPath.Split(@"\");
            CombatLog = split[split.Length - 1];
        }

        public void OpenPlayerAnalysis()
        {
            IsParsing = true;

            Task.Run(() => GetData(_combatLogPath));
        }

        public void Update(string combatInformation)
        {
            FoundCombat = combatInformation;
        }

        public override void ViewAppeared()
        {
            IsParsing = false;
        }

        private async Task GetData(string combatLog)
        {
            var parser = new CombaInformationtParser();
            parser.AddObserver(this);
            await parser.Parse(combatLog);

            var combats = parser.Combats;
            var combatsMapper = _mapper.Map<List<CombatModel>>(combats);
            _combats = combatsMapper;

            await GetDetails();
        }

        private async Task GetDetails()
        {
            for (int i = 0; i < _combats.Count; i++)
            {
                foreach (var item in _combats[i].Players)
                {
                    _combats[i].DamageDone += item.DamageDone;
                    _combats[i].HealDone += item.HealDone;
                    _combats[i].EnergyRecovery += item.EnergyRecovery;
                    _combats[i].DamageTaken += item.DamageTaken;
                }

                if (IsNeedSave)
                {
                    await SaveCombatData(_combats[i]);
                }
            }

            await _mvvmNavigation.Navigate<GeneralAnalysisViewModel, List<CombatModel>>(_combats);
        }

        private async Task SaveCombatData(CombatModel combat)
        {
            IsSaving = true;

            var combatInformation = new CombatDetailsInformation();
            var map = _mapper.Map<Combat>(combat);
            combatInformation.SetData(map);

            var combatResponse = await _httpClient.PostAsync("Combat", JsonContent.Create(combat));
            var createdCombatId = combatResponse.Content.ReadFromJsonAsync<int>().Result;

            for (int i = 0; i < combat.Players.Count; i++)
            {
                combat.Players[i].CombatId = createdCombatId;

                var combatPlayerResponse = await _httpClient.PostAsync("CombatPlayer", JsonContent.Create(combat.Players[i]));
                var createdCombatPlayerId = combatPlayerResponse.Content.ReadFromJsonAsync<int>().Result;

                combatInformation.SetData(combat.Players[i].UserName);

                combatInformation.GetDamageDone();
                combatInformation.GetHealDone();
                combatInformation.GetResourceRecovery();
                combatInformation.GetDamageTaken();

                await GetDamageDoneDetails(combatInformation, createdCombatPlayerId);
                await GetHealDoneDetails(combatInformation, createdCombatPlayerId);
                await GetResourceRecoveryDetails(combatInformation, createdCombatPlayerId);
                await GetDamageTakenDetails(combatInformation, createdCombatPlayerId);
            }
        }

        private async Task GetDamageDoneDetails(CombatDetailsInformation combatInformation, int combatPlayerId)
        {
            foreach (var item in combatInformation.DamageDone)
            {
                var map1 = _mapper.Map<DamageDoneModel>(item);
                map1.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("DamageDone", JsonContent.Create(map1));
            }
        }

        private async Task GetHealDoneDetails(CombatDetailsInformation combatInformation, int combatPlayerId)
        {
            foreach (var item in combatInformation.HealDone)
            {
                var map1 = _mapper.Map<DamageDoneModel>(item);
                map1.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("DamageDone", JsonContent.Create(map1));
            }
        }

        private async Task GetResourceRecoveryDetails(CombatDetailsInformation combatInformation, int combatPlayerId)
        {
            foreach (var item in combatInformation.ResourceRecovery)
            {
                var map1 = _mapper.Map<DamageDoneModel>(item);
                map1.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("DamageDone", JsonContent.Create(map1));
            }
        }

        private async Task GetDamageTakenDetails(CombatDetailsInformation combatInformation, int combatPlayerId)
        {
            foreach (var item in combatInformation.DamageTaken)
            {
                var map1 = _mapper.Map<DamageDoneModel>(item);
                map1.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("DamageDone", JsonContent.Create(map1));
            }
        }
    }
}
