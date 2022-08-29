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

            foreach (var item in combatPlayers)
            {
                var combatPlayerResponse = await _httpClient.PostAsync("CombatPlayer", JsonContent.Create(item));
                var createdCombatPlayerId = await combatPlayerResponse.Content.ReadFromJsonAsync<int>();

                _details.Initialization(item.UserName);

                _details.Clear();

                _details.GetDamageDone();
                _details.GetHealDone();
                _details.GetResourceRecovery();
                _details.GetDamageTaken();

                await SaveDamageDoneDetails(_details.DamageDone, createdCombatPlayerId);

                var damageDoneGeneralData = _details.GetDamageDoneGeneral(_details.DamageDone, map);
                await SaveDamageDoneGeneral(damageDoneGeneralData.ToList(), createdCombatPlayerId);

                await SaveHealDoneDetails(_details.HealDone, createdCombatPlayerId);

                var healDoneGeneralData = _details.GetHealDoneGeneral(_details.HealDone, map);
                await SaveHealDoneGeneral(healDoneGeneralData.ToList(), createdCombatPlayerId);

                await SaveDamageTakenDetails(_details.DamageTaken, createdCombatPlayerId);

                var damageTakenGeneralData = _details.GetDamageTakenGeneral(_details.DamageTaken, map);
                await SaveDamageTakenGeneral(damageTakenGeneralData.ToList(), createdCombatPlayerId);

                await SaveResourceRecoveryDetails(_details.ResourceRecovery, createdCombatPlayerId);

                var resourceRecoveryGeneralData = _details.GetResourceRecoveryGeneral(_details.ResourceRecovery, map);
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
