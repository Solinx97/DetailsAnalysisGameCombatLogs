using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.CombatParser.Patterns;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CombatAnalysis.CombatParser.Services
{
    public class CombatParserService : IParser
    {
        private readonly IList<IObserver> _observers;
        private readonly IList<PlaceInformation> _zones;
        private readonly IFileManager _fileManager;
        private readonly ILogger _logger;

        private TimeSpan _minCombatDuration = TimeSpan.Parse("00:00:20");

        public CombatParserService(IFileManager fileManager, ILogger logger)
        {
            _fileManager = fileManager;
            _logger = logger;

            Combats = new List<Combat>();
            _observers = new List<IObserver>();
            _zones = new List<PlaceInformation>();
        }

        public List<Combat> Combats { get; }

        public async Task<bool> FileCheck(string combatLog)
        {
            using var reader = _fileManager.StreamReader(combatLog);
            var fileIsCorrect = true;
            var line = await reader.ReadLineAsync();
            if (!line.Contains("COMBAT_LOG_VERSION"))
            {
                fileIsCorrect = false;
            }

            return fileIsCorrect;
        }

        public async Task Parse(string combatLog)
        {
            Combats.Clear();

            using var reader = _fileManager.StreamReader(combatLog);
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (line.Contains("ENCOUNTER_START"))
                {
                    await GetCombatInformation(line, reader);
                }
                else if (line.Contains("ZONE_CHANGE"))
                {
                    GetDungeonName(line);
                }
            }
        }

        private async Task GetCombatInformation(string line, StreamReader reader)
        {
            try
            {
                var combatData = await GetCombatData(reader);
                combatData.Insert(0, line);

                var combat = new Combat
                {
                    Name = GetCombatName(combatData[0]),
                    Data = combatData,
                    IsWin = GetCombatResult(combatData[^1]),
                    StartDate = GetTime(combatData[0]),
                    FinishDate = GetTime(combatData[^1]),
                };

                var duration = combat.FinishDate - combat.StartDate;
                if (duration >= _minCombatDuration)
                {
                    GetCombatPlayersData(combat);

                    CombatDetailsTemplate combatDetailsDeaths = new CombatDetailsDeaths(_logger, combat.Players);
                    combat.DeathNumber = combatDetailsDeaths.GetData(string.Empty, combat.Data);

                    CalculatingCommonCombatDetails(combat);

                    AddNewCombat(combat);
                }
            }
            catch (IndexOutOfRangeException)
            {
                var combat = new Combat();
                AddNewCombat(combat);
            }
        }

        private async Task<List<string>> GetCombatData(StreamReader reader)
        {
            string line;
            var combatElements = new List<string>();
            while ((line = await reader.ReadLineAsync()) != null)
            {
                combatElements.Add(line);
                if (line.Contains("ENCOUNTER_END"))
                {
                    break;
                }
            }

            return combatElements;
        }

        private string GetCombatName(string combatStart)
        {
            var data = combatStart.Split("  ")[1];
            var name = data.Split(',')[2];
            var clearName = name.Trim('"');

            return clearName;
        }

        private bool GetCombatResult(string combatFinish)
        {
            var data = combatFinish.Split("  ");
            var split = data[1].Split(',');
            var combatResult = int.Parse(split[split.Length - 1]);
            var isWin = combatResult == 1;

            return isWin;
        }

        private DateTimeOffset GetTime(string combatStart)
        {
            var parse = combatStart.Split("  ")[0];
            var combatDate = parse.Split(' ');
            var dateWithoutTime = combatDate[0].Split('/');
            var time = combatDate[1].Split('.')[0];
            var clearDate = $"{dateWithoutTime[1]}/{dateWithoutTime[0]}/{DateTimeOffset.Now.Year} {time}";

            DateTimeOffset.TryParse(clearDate, out var date);
            return date.UtcDateTime;
        }

        private void GetCombatPlayersData(Combat combat)
        {
            var combatPlayerDataCollection = new List<CombatPlayer>();

            var players = GetCombatPlayers(combat.Data);
            foreach (var item in players)
            {
                CombatDetailsTemplate combatDetailsDamageDone = new CombatDetailsDamageDone(_logger);
                CombatDetailsTemplate combatDetailsHealDone = new CombatDetailsHealDone(_logger);
                CombatDetailsTemplate combatDetailsDamageTaken = new CombatDetailsDamageTaken(_logger);
                CombatDetailsTemplate combatDetailsResourceRecovery = new CombatDetailsResourceRecovery(_logger);
                var test = combatDetailsResourceRecovery.GetData(item, combat.Data);

                var combatPlayerData = new CombatPlayer
                {
                    UserName = item,
                    EnergyRecovery = combatDetailsResourceRecovery.GetData(item, combat.Data),
                    DamageDone = combatDetailsDamageDone.GetData(item, combat.Data),
                    HealDone = combatDetailsHealDone.GetData(item, combat.Data),
                    DamageTaken = combatDetailsDamageTaken.GetData(item, combat.Data),
                };

                combatPlayerDataCollection.Add(combatPlayerData);
            }

            combat.Players = combatPlayerDataCollection;
        }

        private void CalculatingCommonCombatDetails(Combat combat)
        {
            foreach (var item in combat.Players)
            {
                combat.DamageDone += item.DamageDone;
                combat.HealDone += item.HealDone;
                combat.DamageTaken += item.DamageTaken;
                combat.EnergyRecovery += item.EnergyRecovery;
            }
        }

        private void AddNewCombat(Combat combat)
        {
            foreach (var item in _zones)
            {
                if (item.EntryDate < combat.StartDate)
                {
                    combat.DungeonName = item.Name;
                }
            }

            Combats.Add(combat);

            NotifyObservers();
        }

        private List<string> GetCombatPlayers(List<string> combatInformation)
        {
            var playersId = new List<string>();

            for (int i = 1; i < combatInformation.Count; i++)
            {
                if (combatInformation[i].Contains("COMBATANT_INFO"))
                {
                    var data = combatInformation[i].Split("  ")[1];
                    playersId.Add(data.Split(',')[1]);
                }
                else break;
            }

            var players = new List<string>();
            foreach (var item in playersId)
            {
                var player = GetCombatPlayersById(combatInformation, item);
                players.Add(player);
            }

            return players;
        }

        private void GetDungeonName(string combatLog)
        {
            var parse = combatLog.Split("  ")[1];
            var name = parse.Split(',')[2];
            var clearName = name.Trim('"');

            var date = GetTime(combatLog);

            var zone = new PlaceInformation
            {
                Name = clearName,
                EntryDate = date
            };

            _zones.Add(zone);
        }

        private string GetCombatPlayersById(List<string> combatInformation, string playerId)
        {
            var player = string.Empty;
            for (int i = 1; i < combatInformation.Count; i++)
            {
                var data = combatInformation[i].Split(',');
                if (!combatInformation[i].Contains("COMBATANT_INFO")
                    && playerId == data[1])
                {
                    var userName = data[2];
                    player = userName.Trim('"');
                    break;
                }
            }

            return player;
        }

        public void AddObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                var isWin = Combats[^1].IsWin ? "Победа" : "Поражение";
                var combatInformation = $"Подземелье: {Combats[^1].DungeonName}, Бой: {Combats[^1].Name}, Время: {Combats[^1].Duration}, Результат: {isWin}";

                observer.Update(combatInformation);
            }
        }
    }
}
