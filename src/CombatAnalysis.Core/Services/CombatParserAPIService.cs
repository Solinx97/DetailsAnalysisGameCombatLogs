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
using static CombatAnalysis.Core.ViewModels.MainInformationViewModel;

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

        public async Task<int> SaveCombatLog()
        {
            var combatLog = CreateCombatLog();
            var combatLogResponse = await _httpClient.PostAsync("CombatLog", JsonContent.Create(combatLog));
            var createdCombatLogId = combatLogResponse.Content.ReadFromJsonAsync<int>().Result;

            return createdCombatLogId;
        }

        public async Task DeleteCombatLog(int id, Temporary temp)
        {
            var tasks = new List<Task>();
            var combats = await LoadCombats(id);
            foreach (var item in combats)
            {
                tasks.Add(DeleteCombatPlayersData(item.Id, temp));
                tasks.Add(_httpClient.DeletAsync($"Combat/{item.Id}"));
            }

            await Task.WhenAll(tasks);

            await _httpClient.DeletAsync($"CombatLog/{id}");
        }

        public async Task<IEnumerable<CombatLogModel>> LoadCombatLogs()
        {
            var responseMessage = await _httpClient.GetAsync("CombatLog");
            var combatLogs = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatLogModel>>();

            return combatLogs;
        }

        public async Task<IEnumerable<CombatModel>> LoadCombats(int combatLogId)
        {
            var responseMessage = await _httpClient.GetAsync($"Combat/FindByCombatLogId/{combatLogId}");
            var combats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatModel>>();

            return combats;
        }

        public async Task<IEnumerable<CombatPlayerDataModel>> LoadCombatPlayers(int combatId)
        {
            var responseMessage = await _httpClient.GetAsync($"CombatPlayer/FindByCombatId/{combatId}");
            var combatPlayers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerDataModel>>();

            return combatPlayers;
        }

        public async Task<IEnumerable<HealDoneModel>> LoadHealDoneDetails(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"HealDone/FindByCombatPlayerId/{combatPlayerId}");
            var healDones = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<HealDoneModel>>();

            return healDones;
        }

        public async Task<IEnumerable<HealDoneGeneralModel>> LoadHealDoneGeneral(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"HealDoneGeneral/FindByCombatPlayerId/{combatPlayerId}");
            var healDoneGenerics = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<HealDoneGeneralModel>>();

            return healDoneGenerics;
        }

        public async Task<IEnumerable<DamageDoneModel>> LoadDamageDoneDetails(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"DamageDone/FindByCombatPlayerId/{combatPlayerId}");
            var damageDones = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageDoneModel>>();

            return damageDones;
        }

        public async Task<IEnumerable<DamageDoneGeneralModel>> LoadDamageDoneGeneral(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"DamageDoneGeneral/FindByCombatPlayerId/{combatPlayerId}");
            var damageDoneGenerics = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageDoneGeneralModel>>();

            return damageDoneGenerics;
        }

        public async Task<IEnumerable<DamageTakenModel>> LoadDamageTakenDetails(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"DamageTaken/FindByCombatPlayerId/{combatPlayerId}");
            var damageTakens = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageTakenModel>>();

            return damageTakens;
        }

        public async Task<IEnumerable<DamageTakenGeneralModel>> LoadDamageTakenGeneral(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"DamageTakenGeneral/FindByCombatPlayerId/{combatPlayerId}");
            var damageTakenGenerals = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageTakenGeneralModel>>();

            return damageTakenGenerals;
        }

        public async Task<IEnumerable<ResourceRecoveryModel>> LoadResourceRecoveryDetails(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"ResourceRecovery/FindByCombatPlayerId/{combatPlayerId}");
            var resourceRecoveryes = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<ResourceRecoveryModel>>();

            return resourceRecoveryes;
        }

        public async Task<IEnumerable<ResourceRecoveryGeneralModel>> LoadResourceRecoveryGeneral(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"ResourceRecoveryGeneral/FindByCombatPlayerId/{combatPlayerId}");
            var ResourceRecoveryGenerals = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<ResourceRecoveryGeneralModel>>();

            return ResourceRecoveryGenerals;
        }

        public async Task SaveCombatData(CombatModel combat, int createdCombatLogId)
        {
            var combatInformation = new CombatDetailsInformation();
            var map = _mapper.Map<Combat>(combat);
            combatInformation.SetData(map);

            combat.CombatLogId = createdCombatLogId;
            var combatResponse = await _httpClient.PostAsync("Combat", JsonContent.Create(combat));
            var createdCombatId = combatResponse.Content.ReadFromJsonAsync<int>().Result;

            await SaveCombatPlayerData(combatInformation, combat, createdCombatId);
        }

        private async Task DeleteCombatPlayersData(int combatId, Temporary temp)
        {
            var players = await LoadCombatPlayers(combatId);
            foreach (var item in players)
            {
                await DeleteHealDoneData(item.Id, temp);
                await DeleteHealDoneGeneralData(item.Id, temp);
                await DeleteDamageDoneData(item.Id, temp);
                await DeleteDamageDoneGeneralData(item.Id, temp);
                await DeleteDamageTakenData(item.Id, temp);
                await DeleteDamageTakenGeneralData(item.Id, temp);
                await DeleteResourceRecoveryData(item.Id, temp);
                await DeleteResourceRecoveryGeneralData(item.Id, temp);

                await _httpClient.DeletAsync($"CombatPlayer/{item.Id}");
            }
        }

        private async Task DeleteHealDoneData(int combatPlayerId, Temporary temp)
        {
            var healDones = await LoadHealDoneDetails(combatPlayerId);
            foreach (var item2 in healDones)
            {
                var response = await _httpClient.DeletAsync($"HealDone/{item2.Id}");
                int count = await response.Content.ReadFromJsonAsync<int>();
                temp?.Invoke(count);
            }
        }

        private async Task DeleteHealDoneGeneralData(int combatPlayerId, Temporary temp)
        {
            var healDoneGenerals = await LoadHealDoneGeneral(combatPlayerId);
            foreach (var item2 in healDoneGenerals)
            {
                var response = await _httpClient.DeletAsync($"HealDoneGeneral/{item2.Id}");
                int count = await response.Content.ReadFromJsonAsync<int>();
                temp?.Invoke(count);
            }
        }

        private async Task DeleteDamageDoneData(int combatPlayerId, Temporary temp)
        {
            var damageDones = await LoadDamageDoneDetails(combatPlayerId);
            foreach (var item2 in damageDones)
            {
                var response = await _httpClient.DeletAsync($"DamageDone/{item2.Id}");
                int count = await response.Content.ReadFromJsonAsync<int>();
                temp?.Invoke(count);
            }
        }

        private async Task DeleteDamageDoneGeneralData(int combatPlayerId, Temporary temp)
        {
            var damageDoneGenerals = await LoadDamageDoneGeneral(combatPlayerId);
            foreach (var item2 in damageDoneGenerals)
            {
                var response = await _httpClient.DeletAsync($"DamageDoneGeneral/{item2.Id}");
                int count = await response.Content.ReadFromJsonAsync<int>();
                temp?.Invoke(count);
            }
        }

        private async Task DeleteDamageTakenData(int combatPlayerId, Temporary temp)
        {
            var damageTakens = await LoadDamageTakenDetails(combatPlayerId);
            foreach (var item2 in damageTakens)
            {
                var response = await _httpClient.DeletAsync($"DamageTaken/{item2.Id}");
                int count = await response.Content.ReadFromJsonAsync<int>();
                temp?.Invoke(count);
            }
        }

        private async Task DeleteDamageTakenGeneralData(int combatPlayerId, Temporary temp)
        {
            var damageTakenGenerals = await LoadDamageTakenGeneral(combatPlayerId);
            foreach (var item2 in damageTakenGenerals)
            {
                var response = await _httpClient.DeletAsync($"DamageTakenGeneral/{item2.Id}");
                int count = await response.Content.ReadFromJsonAsync<int>();
                temp?.Invoke(count);
            }
        }

        private async Task DeleteResourceRecoveryData(int combatPlayerId, Temporary temp)
        {
            var resourceRecoveryes = await LoadResourceRecoveryDetails(combatPlayerId);
            foreach (var item2 in resourceRecoveryes)
            {
                var response = await _httpClient.DeletAsync($"ResourceRecovery/{item2.Id}");
                int count = await response.Content.ReadFromJsonAsync<int>();
                temp?.Invoke(count);
            }
        }

        private async Task DeleteResourceRecoveryGeneralData(int combatPlayerId, Temporary temp)
        {
            var resourceRecoveryGenerals = await LoadResourceRecoveryGeneral(combatPlayerId);
            foreach (var item2 in resourceRecoveryGenerals)
            {
                var response = await _httpClient.DeletAsync($"ResourceRecoveryGeneral/{item2.Id}");
                int count = await response.Content.ReadFromJsonAsync<int>();
                temp?.Invoke(count);
            }
        }

        private CombatLogModel CreateCombatLog()
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

            return combatLog;
        }

        private async Task SaveCombatPlayerData(CombatDetailsInformation combatInformation, CombatModel combat, int createdCombatId)
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
                await Task.Run(() => SaveDamageDoneDetails(damageDoneData, createdCombatPlayerId));

                var damageDoneGeneralData = combatInformation.GetDamageDoneGeneral(damageDoneData, map);
                await Task.Run(() => SaveDamageDoneGeneral(damageDoneGeneralData.ToList(), createdCombatPlayerId));

                var healDoneData = new List<HealDone>(combatInformation.HealDone);
                await Task.Run(() => SaveHealDoneDetails(healDoneData, createdCombatPlayerId));

                var healDoneGeneralData = combatInformation.GetHealDoneGeneral(healDoneData, map);
                await Task.Run(() => SaveHealDoneGeneral(healDoneGeneralData.ToList(), createdCombatPlayerId));

                var damageTakenData = new List<DamageTaken>(combatInformation.DamageTaken);
                await Task.Run(() => SaveDamageTakenDetails(damageTakenData, createdCombatPlayerId));

                var damageTakenGeneralData = combatInformation.GetDamageTakenGeneral(damageTakenData, map);
                await Task.Run(() => SaveDamageTakenGeneral(damageTakenGeneralData.ToList(), createdCombatPlayerId));

                var resourceRecoveryData = new List<ResourceRecovery>(combatInformation.ResourceRecovery);
                await Task.Run(() => SaveResourceRecoveryDetails(resourceRecoveryData, createdCombatPlayerId));

                var resourceRecoveryGeneralData = combatInformation.GetResourceRecoveryGeneral(resourceRecoveryData, map);
                await Task.Run(() => SaveResourceRecoveryGeneral(resourceRecoveryGeneralData.ToList(), createdCombatPlayerId));
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

        private async Task SaveResourceRecoveryGeneral(List<ResourceRecoveryGeneral> resourceRecoveryGeneral, int combatPlayerId)
        {
            foreach (var item in resourceRecoveryGeneral)
            {
                var map1 = _mapper.Map<ResourceRecoveryGeneralModel>(item);
                map1.CombatPlayerDataId = combatPlayerId;

                await _httpClient.PostAsync("ResourceRecoveryGeneral", JsonContent.Create(map1));
            }
        }
    }
}
