using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.Services
{
    internal class CombatParserAPIService
    {
        private readonly IHttpClientHelper _httpClient;
        private readonly ILogger _logger;

        private List<CombatModel> _combats;

        public CombatParserAPIService(IHttpClientHelper httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public void SetCombats(List<CombatModel> combats)
        {
            _combats = combats;
        }

        public async Task<bool> Save(List<CombatModel> combats)
        {
            try
            {
                SetCombats(combats);
                var createdCombatLogId = await SaveCombatLogAsync().ConfigureAwait(false);

                foreach (var item in combats)
                {
                    await SaveCombatDataAsync(item, createdCombatLogId).ConfigureAwait(false);
                }

                await SetReadyForCombatLog(createdCombatLogId).ConfigureAwait(false);

                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, ex.Message);

                return false;
            }
        }

        public async Task<int> SaveCombatLogAsync()
        {
            var dungeonNames = _combats
                 .GroupBy(group => group.DungeonName)
                 .Select(select => select.Key)
                 .ToList();

            var combatLogResponse = await _httpClient.PostAsync("CombatLog", JsonContent.Create(dungeonNames));
            var createdCombatLogId = await combatLogResponse.Content.ReadFromJsonAsync<int>();

            return createdCombatLogId;
        }

        public async Task SaveCombatDataAsync(CombatModel combat, int createdCombatLogId)
        {
            combat.CombatLogId = createdCombatLogId;
            var combatDataResponse = await _httpClient.PostAsync("Combat", JsonContent.Create(combat));
            var createdCombatId = await combatDataResponse.Content.ReadFromJsonAsync<int>();

            foreach (var item in combat.Players)
            {
                item.CombatId = createdCombatId;
            }

            await _httpClient.PostAsync("Combat/SaveCombatPlayers", JsonContent.Create(combat.Players));
        }   

        public async Task SetReadyForCombatLog(int createdCombatLogId)
        {
            var combatLogResponse = await _httpClient.GetAsync($"CombatLog/{createdCombatLogId}");
            var combatLog = await combatLogResponse.Content.ReadFromJsonAsync<CombatLogModel>();
            combatLog.IsReady = true;

            await _httpClient.PutAsync("CombatLog", JsonContent.Create(combatLog));
        }

        public async Task DeleteCombatLogAsync(int id)
        {
            var combats = await LoadCombatsAsync(id).ConfigureAwait(false);
            foreach (var item in combats)
            {
                await DeleteCombatPlayersData(item.Id).ConfigureAwait(false);
                await _httpClient.DeletAsync($"Combat/{item.Id}").ConfigureAwait(false);
            }

            await _httpClient.DeletAsync($"CombatLog/{id}").ConfigureAwait(false);
        }

        public async Task<IEnumerable<CombatLogModel>> LoadCombatLogsAsync()
        {
            var responseMessage = await _httpClient.GetAsync("CombatLog");
            var combatLogs = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatLogModel>>();

            return combatLogs;
        }

        public async Task<IEnumerable<CombatModel>> LoadCombatsAsync(int combatLogId)
        {
            var responseMessage = await _httpClient.GetAsync($"Combat/FindByCombatLogId/{combatLogId}");
            var combats = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatModel>>();

            return combats;
        }

        public async Task<IEnumerable<CombatPlayerDataModel>> LoadCombatPlayersAsync(int combatId)
        {
            var responseMessage = await _httpClient.GetAsync($"CombatPlayer/FindByCombatId/{combatId}");
            var combatPlayers = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerDataModel>>();

            return combatPlayers;
        }

        public async Task<IEnumerable<HealDoneModel>> LoadHealDoneDetailsAsync(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"HealDone/FindByCombatPlayerId/{combatPlayerId}");
            var healDones = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<HealDoneModel>>();

            return healDones;
        }

        public async Task<IEnumerable<HealDoneGeneralModel>> LoadHealDoneGeneralAsync(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"HealDoneGeneral/FindByCombatPlayerId/{combatPlayerId}");
            var healDoneGenerics = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<HealDoneGeneralModel>>();

            return healDoneGenerics;
        }

        public async Task<IEnumerable<DamageDoneModel>> LoadDamageDoneDetailsAsync(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"DamageDone/FindByCombatPlayerId/{combatPlayerId}");
            var damageDones = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageDoneModel>>();

            return damageDones;
        }

        public async Task<IEnumerable<DamageDoneGeneralModel>> LoadDamageDoneGeneralAsync(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"DamageDoneGeneral/FindByCombatPlayerId/{combatPlayerId}");
            var damageDoneGenerics = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageDoneGeneralModel>>();

            return damageDoneGenerics;
        }

        public async Task<IEnumerable<DamageTakenModel>> LoadDamageTakenDetailsAsync(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"DamageTaken/FindByCombatPlayerId/{combatPlayerId}");
            var damageTakens = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageTakenModel>>();

            return damageTakens;
        }

        public async Task<IEnumerable<DamageTakenGeneralModel>> LoadDamageTakenGeneralAsync(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"DamageTakenGeneral/FindByCombatPlayerId/{combatPlayerId}");
            var damageTakenGenerals = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<DamageTakenGeneralModel>>();

            return damageTakenGenerals;
        }

        public async Task<IEnumerable<ResourceRecoveryModel>> LoadResourceRecoveryDetailsAsync(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"ResourceRecovery/FindByCombatPlayerId/{combatPlayerId}");
            var resourceRecoveryes = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<ResourceRecoveryModel>>();

            return resourceRecoveryes;
        }

        public async Task<IEnumerable<ResourceRecoveryGeneralModel>> LoadResourceRecoveryGeneralAsync(int combatPlayerId)
        {
            var responseMessage = await _httpClient.GetAsync($"ResourceRecoveryGeneral/FindByCombatPlayerId/{combatPlayerId}");
            var ResourceRecoveryGenerals = await responseMessage.Content.ReadFromJsonAsync<IEnumerable<ResourceRecoveryGeneralModel>>();

            return ResourceRecoveryGenerals;
        }

        private async Task DeleteCombatPlayersData(int combatId)
        {
            var players = await LoadCombatPlayersAsync(combatId);
            foreach (var item in players)
            {
                await _httpClient.DeletAsync($"HealDone/DeleteByCombatPlayerId/{item.Id}");
                await _httpClient.DeletAsync($"HealDoneGeneral/DeleteByCombatPlayerId/{item.Id}");
                await _httpClient.DeletAsync($"DamageDone/DeleteByCombatPlayerId/{item.Id}");
                await _httpClient.DeletAsync($"DamageDoneGeneral/DeleteByCombatPlayerId/{item.Id}");
                await _httpClient.DeletAsync($"DamageTaken/DeleteByCombatPlayerId/{item.Id}");
                await _httpClient.DeletAsync($"DamageTakenGeneral/DeleteByCombatPlayerId/{item.Id}");
                await _httpClient.DeletAsync($"ResourceRecovery/DeleteByCombatPlayerId/{item.Id}");
                await _httpClient.DeletAsync($"ResourceRecoveryGeneral/DeleteByCombatPlayerId/{item.Id}");

                await _httpClient.DeletAsync($"CombatPlayer/{item.Id}");
            }
        }
    }
}
