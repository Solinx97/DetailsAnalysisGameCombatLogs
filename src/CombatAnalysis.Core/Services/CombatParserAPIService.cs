using AutoMapper;
using CombatAnalysis.CombatParser;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.Services
{
    internal class CombatParserAPIService
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientHelper _httpClient;

        private List<CombatModel> _combats;

        public CombatParserAPIService(IMapper mapper, IHttpClientHelper httpClient)
        {
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public void SetCombats(List<CombatModel> combats)
        {
            _combats = combats;
        }

        public async Task<int> SaveCombatLogData()
        {
            var dungeonNames = _combats
                .GroupBy(group => group.DungeonName)
                .Select(select => select.ToList())
                .ToList();

            var combatLogDungeonName = new StringBuilder();
            foreach (var item in dungeonNames)
            {
                var dungeonName = item.First().DungeonName.Trim('"');
                combatLogDungeonName.Append($"{dungeonName}/");
            }

            combatLogDungeonName.Remove(combatLogDungeonName.Length - 1, 1);

            var combatLog = new CombatLogModel
            {
                Name = combatLogDungeonName.ToString(),
                Date = DateTimeOffset.Now
            };

            var combatLogResponse = await _httpClient.PostAsync("CombatLog", JsonContent.Create(combatLog));
            var createdCombatLogId = combatLogResponse.Content.ReadFromJsonAsync<int>().Result;

            return createdCombatLogId;
        }

        public async Task SaveCombatData(CombatModel combat, int createdCombatLogId)
        {
            var combatInformation = new CombatDetailsInformation();
            var map = _mapper.Map<Combat>(combat);
            combatInformation.SetData(map);

            combat.CombatLogId = createdCombatLogId;
            var combatResponse = await _httpClient.PostAsync("Combat", JsonContent.Create(combat));
            var createdCombatId = combatResponse.Content.ReadFromJsonAsync<int>().Result;

            var tasks = new List<Task>();
            await SaveCombatPlayerData(combatInformation, combat, createdCombatId, tasks);

            foreach (var item in tasks)
            {
                item.Start();
            }

            Task.WaitAll(tasks.ToArray());
        }

        private async Task SaveCombatPlayerData(CombatDetailsInformation combatInformation, CombatModel combat, int createdCombatId, List<Task> tasks)
        {
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

                var map = _mapper.Map<Combat>(combat);

                var damageDoneData = new List<DamageDone>(combatInformation.DamageDone);
                var damageDoneTask = new Task(async () => await SaveDamageDoneDetails(damageDoneData, createdCombatPlayerId));
                tasks.Add(damageDoneTask);

                var damageDoneGeneralData = combatInformation.GetDamageDoneGeneral(damageDoneData, map);
                var damageDoneGeneralTask = new Task(async () => await SaveDamageDoneGeneral(damageDoneGeneralData.ToList(), createdCombatPlayerId));
                tasks.Add(damageDoneGeneralTask);

                var healDoneData = new List<HealDone>(combatInformation.HealDone);
                var healDoneTask = new Task(async () => await SaveHealDoneDetails(healDoneData, createdCombatPlayerId));
                tasks.Add(healDoneTask);

                var healDoneGeneralData = combatInformation.GetHealDoneGeneral(healDoneData, map);
                var healDoneGeneralTask = new Task(async () => await SaveHealDoneGeneral(healDoneGeneralData.ToList(), createdCombatPlayerId));
                tasks.Add(healDoneGeneralTask);

                var damageTakenData = new List<DamageTaken>(combatInformation.DamageTaken);
                var damageTakenTask = new Task(async () => await SaveDamageTakenDetails(damageTakenData, createdCombatPlayerId));
                tasks.Add(damageTakenTask);

                var damageTakenGeneralData = combatInformation.GetDamageTakenGeneral(damageTakenData, map);
                var damageTakenGeneralTask = new Task(async () => await SaveDamageTakenGeneral(damageTakenGeneralData.ToList(), createdCombatPlayerId));
                tasks.Add(damageTakenGeneralTask);

                var resourceRecoveryData = new List<ResourceRecovery>(combatInformation.ResourceRecovery);
                var resourceRecoveryTask = new Task(async () => await SaveResourceRecoveryDetails(resourceRecoveryData, createdCombatPlayerId));
                tasks.Add(resourceRecoveryTask);
            }
        }

        private async Task SaveDamageDoneDetails(List<DamageDone> damageDone, int combatPlayerId)
        {
            foreach (var item in damageDone)
            {
                var map1 = _mapper.Map<DamageDoneModel>(item);
                map1.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("DamageDone", JsonContent.Create(map1));
            }
        }

        private async Task SaveDamageDoneGeneral(List<DamageDoneGeneral> damageDoneGeneral, int combatPlayerId)
        {
            foreach (var item in damageDoneGeneral)
            {
                var map1 = _mapper.Map<DamageDoneGeneralModel>(item);
                map1.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("DamageDoneGeneral", JsonContent.Create(map1));
            }
        }

        private async Task SaveHealDoneDetails(List<HealDone> healDone, int combatPlayerId)
        {
            foreach (var item in healDone)
            {
                var map1 = _mapper.Map<HealDoneModel>(item);
                map1.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("HealDone", JsonContent.Create(map1));
            }
        }

        private async Task SaveHealDoneGeneral(List<HealDoneGeneral> healDoneGeneral, int combatPlayerId)
        {
            foreach (var item in healDoneGeneral)
            {
                var map1 = _mapper.Map<HealDoneGeneralModel>(item);
                map1.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("HealDoneGeneral", JsonContent.Create(map1));
            }
        }

        private async Task SaveDamageTakenDetails(List<DamageTaken> damageTaken, int combatPlayerId)
        {
            foreach (var item in damageTaken)
            {
                var map1 = _mapper.Map<DamageTakenModel>(item);
                map1.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("DamageTaken", JsonContent.Create(map1));
            }
        }

        private async Task SaveDamageTakenGeneral(List<DamageTakenGeneral> damageTaken, int combatPlayerId)
        {
            foreach (var item in damageTaken)
            {
                var map1 = _mapper.Map<DamageTakenGeneralModel>(item);
                map1.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("DamageTakenGeneral", JsonContent.Create(map1));
            }
        }

        private async Task SaveResourceRecoveryDetails(List<ResourceRecovery> resourceRecovery, int combatPlayerId)
        {
            foreach (var item in resourceRecovery)
            {
                var map1 = _mapper.Map<ResourceRecoveryModel>(item);
                map1.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("ResourceRecovery", JsonContent.Create(map1));
            }
        }
    }
}
