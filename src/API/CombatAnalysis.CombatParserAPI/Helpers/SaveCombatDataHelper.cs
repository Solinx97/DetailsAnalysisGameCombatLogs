using AutoMapper;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CombatAnalysis.CombatParserAPI.Helpers
{
    public class SaveCombatDataHelper
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientHelper _httpClient;
        private readonly ICombatDetails _details;

        public SaveCombatDataHelper(IMapper mapper, IHttpClientHelper httpClient, ICombatDetails details)
        {
            _mapper = mapper;
            _httpClient = httpClient;
            _details = details;
        }

        public static List<string> CombatData;

        public CombatLogModel CreateCombatLog(List<string> dungeonNames)
        {
            var combatLogDungeonName = new StringBuilder();
            foreach (var item in dungeonNames)
            {
                var dungeonName = item.Trim('"');
                combatLogDungeonName.Append($"{dungeonName}/");
            }

            combatLogDungeonName.Remove(combatLogDungeonName.Length - 1, 1);

            var combatLog = new CombatLogModel
            {
                Name = combatLogDungeonName.ToString(),
                Date = DateTimeOffset.Now
            };

            return combatLog;
        }

        public async Task SaveCombatPlayerData(CombatModel combat, List<CombatPlayerDataModel> combatPlayers)
        {
            var map = _mapper.Map<Combat>(combat);
            _details.Initialization(map);

            for (int i = 0; i < combatPlayers.Count; i++)
            {
                var combatPlayerResponse = await _httpClient.PostAsync("CombatPlayer", JsonContent.Create(combatPlayers[i]));
                var createdCombatPlayerId = await combatPlayerResponse.Content.ReadFromJsonAsync<int>();

                _details.Initialization(combatPlayers[i].UserName);

                _details.GetDamageDone();
                _details.GetHealDone();
                _details.GetResourceRecovery();
                _details.GetDamageTaken();

                var damageDoneData = new List<DamageDone>(_details.DamageDone);
                await SaveDamageDoneDetails(damageDoneData, createdCombatPlayerId);

                var damageDoneGeneralData = _details.GetDamageDoneGeneral(damageDoneData, map);
                await SaveDamageDoneGeneral(damageDoneGeneralData.ToList(), createdCombatPlayerId);

                var healDoneData = new List<HealDone>(_details.HealDone);
                await SaveHealDoneDetails(healDoneData, createdCombatPlayerId);

                var healDoneGeneralData = _details.GetHealDoneGeneral(healDoneData, map);
                await SaveHealDoneGeneral(healDoneGeneralData.ToList(), createdCombatPlayerId);

                var damageTakenData = new List<DamageTaken>(_details.DamageTaken);
                await SaveDamageTakenDetails(damageTakenData, createdCombatPlayerId);

                var damageTakenGeneralData = _details.GetDamageTakenGeneral(damageTakenData, map);
                await SaveDamageTakenGeneral(damageTakenGeneralData.ToList(), createdCombatPlayerId);

                var resourceRecoveryData = new List<ResourceRecovery>(_details.ResourceRecovery);
                await SaveResourceRecoveryDetails(resourceRecoveryData, createdCombatPlayerId);

                var resourceRecoveryGeneralData = _details.GetResourceRecoveryGeneral(resourceRecoveryData, map);
                await SaveResourceRecoveryGeneral(resourceRecoveryGeneralData.ToList(), createdCombatPlayerId);
            }
        }

        private async Task SaveDamageDoneDetails(List<DamageDone> damageDone, int combatPlayerId)
        {
            foreach (var item in damageDone)
            {
                var map = _mapper.Map<DamageDoneModel>(item);
                map.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("DamageDone", JsonContent.Create(map));
            }
        }

        private async Task SaveDamageDoneGeneral(List<DamageDoneGeneral> damageDoneGeneral, int combatPlayerId)
        {
            foreach (var item in damageDoneGeneral)
            {
                var map = _mapper.Map<DamageDoneGeneralModel>(item);
                map.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("DamageDoneGeneral", JsonContent.Create(map));
            }
        }

        private async Task SaveHealDoneDetails(List<HealDone> healDone, int combatPlayerId)
        {
            foreach (var item in healDone)
            {
                var map = _mapper.Map<HealDoneModel>(item);
                map.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("HealDone", JsonContent.Create(map));
            }
        }

        private async Task SaveHealDoneGeneral(List<HealDoneGeneral> healDoneGeneral, int combatPlayerId)
        {
            foreach (var item in healDoneGeneral)
            {
                var map = _mapper.Map<HealDoneGeneralModel>(item);
                map.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("HealDoneGeneral", JsonContent.Create(map));
            }
        }

        private async Task SaveDamageTakenDetails(List<DamageTaken> damageTaken, int combatPlayerId)
        {
            foreach (var item in damageTaken)
            {
                var map = _mapper.Map<DamageTakenModel>(item);
                map.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("DamageTaken", JsonContent.Create(map));
            }
        }

        private async Task SaveDamageTakenGeneral(List<DamageTakenGeneral> damageTaken, int combatPlayerId)
        {
            foreach (var item in damageTaken)
            {
                var map = _mapper.Map<DamageTakenGeneralModel>(item);
                map.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("DamageTakenGeneral", JsonContent.Create(map));
            }
        }

        private async Task SaveResourceRecoveryDetails(List<ResourceRecovery> resourceRecovery, int combatPlayerId)
        {
            foreach (var item in resourceRecovery)
            {
                var map = _mapper.Map<ResourceRecoveryModel>(item);
                map.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("ResourceRecovery", JsonContent.Create(map));
            }
        }

        private async Task SaveResourceRecoveryGeneral(List<ResourceRecoveryGeneral> resourceRecoveryGeneral, int combatPlayerId)
        {
            foreach (var item in resourceRecoveryGeneral)
            {
                var map = _mapper.Map<ResourceRecoveryGeneralModel>(item);
                map.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("ResourceRecoveryGeneral", JsonContent.Create(map));
            }
        }
    }
}
