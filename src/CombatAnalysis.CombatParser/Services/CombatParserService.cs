using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Interfaces;
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
        private readonly ICombatDetails _combatDetails;
        private readonly IFileManager _fileManager;

        public CombatParserService(ICombatDetails combatDetails, IFileManager fileManager)
        {
            _combatDetails = combatDetails;
            _fileManager = fileManager;

            Combats = new List<Combat>();
            _observers = new List<IObserver>();
            _zones = new List<PlaceInformation>();
        }

        public List<Combat> Combats { get; }

        public async Task Parse(string combatLog)
        {
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

                GetCombatPlayersData(combat);
                combat.DeathNumber = _combatDetails.GetDeathsNumber();

                CalculatingCommonCombatDetails(combat);

                AddNewCombat(combat);
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
            var clearDate = $"{dateWithoutTime[0]}/{dateWithoutTime[1]}/{DateTimeOffset.UtcNow.Year} {combatDate[1]}";

            DateTimeOffset.TryParse(clearDate, out var date);
            return date.UtcDateTime;
        }

        private void GetCombatPlayersData(Combat combat)
        {
            var playersData = new List<CombatPlayerData>();

            var players = GetCombatPlayers(combat.Data);
            foreach (var item in players)
            {
                _combatDetails.SetData(combat, item);
                var playerCombatData = new CombatPlayerData
                {
                    UserName = item,
                    EnergyRecovery = _combatDetails.GetResourceRecovery(),
                    DamageDone = _combatDetails.GetDamageDone(),
                    HealDone = _combatDetails.GetHealDone(),
                    DamageTaken = _combatDetails.GetDamageTaken(),
                };

                playersData.Add(playerCombatData);
            }

            combat.Players = playersData;
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

        private List<string> GetCombatPlayers(List<string> combat)
        {
            var playersId = new List<string>();

            for (int i = 1; i < combat.Count; i++)
            {
                if (combat[i].Contains("COMBATANT_INFO"))
                {
                    var data = combat[i].Split("  ")[1];
                    playersId.Add(data.Split(',')[1]);
                }
                else break;
            }

            var players = new List<string>();
            foreach (var item in playersId)
            {
                var player = GetCombatPlayersById(combat, item);
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

        private string GetCombatPlayersById(List<string> combat, string playerId)
        {
            var player = string.Empty;
            for (int i = 1; i < combat.Count; i++)
            {
                if (!combat[i].Contains("COMBATANT_INFO"))
                {
                    var data = combat[i].Split(',');
                    var indexPlayerId = Array.IndexOf(data, playerId);
                    if (indexPlayerId > 0)
                    {
                        var userName = data[indexPlayerId + 1];
                        player = userName.Trim('"');
                        break;
                    }
                }
            }

            return player;
        }

        public void AddObserver(IObserver o)
        {
            _observers.Add(o);
        }

        public void RemoveObserver(IObserver o)
        {
            _observers.Remove(o);
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
