using CombatAnalysis.CombatParser.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CombatAnalysis.CombatParser
{
    public class CombatInformation
    {
        private Combat _combat;
        private string _player;

        public CombatInformation()
        {
            _combat = new Combat();
            DamageDoneInformations = new List<DamageDone>();
            HealDoneInformations = new List<HealDone>();
            DamageTakenInformations = new List<DamageTaken>();
        }

        public List<DamageDone> DamageDoneInformations { get; private set; }

        public List<HealDone> HealDoneInformations { get; private set; }

        public List<DamageTaken> DamageTakenInformations { get; private set; }

        public void SetCombat(Combat combat, string player)
        {
            _combat = combat;
            _player = player;
        }

        public void SetCombat(Combat combat)
        {
            _combat = combat;
        }

        public double GetEnergyRecovery()
        {
            double energyRecovery = 0;
            foreach (var item in _combat.Data)
            {
                if ((item.Contains("SPELL_PERIODIC_ENERGIZE") || item.Contains("SPELL_ENERGIZE"))
                    && item.Contains(_player))
                {
                    var combatData = CombatDataParse(item);
                    var energyRecoveryInformation = GetEnergyInformation(combatData.ToArray());
                    energyRecovery += energyRecoveryInformation.Value;
                }
            }

            return energyRecovery;
        }

        public int GetDamageDone()
        {
            int damageDone = 0;
            foreach (var item in _combat.Data)
            {
                if ((item.Contains("SPELL_DAMAGE") || item.Contains("SWING_DAMAGE")
                    || item.Contains("SPELL_PERIODIC_DAMAGE") || item.Contains("SWING_MISSED")
                    || item.Contains("DAMAGE_SHIELD_MISSED") || item.Contains("RANGE_DAMAGE")
                    || item.Contains("SPELL_MISSED")) && item.Contains(_player))
                {
                    var combatData = CombatDataParse(item);
                    var damageDoneInformation = GetDamageDoneInformation(combatData.ToArray());

                    if (damageDoneInformation != null)
                    {
                        damageDone += damageDoneInformation.Value;
                        DamageDoneInformations.Add(damageDoneInformation);
                    }
                }
            }

            return damageDone;
        }

        public int GetHealDone()
        {
            int healthDone = 0;
            foreach (var item in _combat.Data)
            {
                if ((item.Contains("SPELL_HEAL") || item.Contains("SPELL_PERIODIC_HEAL"))
                    && item.Contains(_player))
                {
                    var combatData = CombatDataParse(item);
                    var healDoneInformation = GetHealDoneInformation(combatData.ToArray());

                    if (healDoneInformation != null)
                    {
                        healthDone += healDoneInformation.Value;
                        HealDoneInformations.Add(healDoneInformation);
                    }
                }
            }

            return healthDone;
        }

        public int GetDamageTaken()
        {
            int damageTaken = 0;
            foreach (var item in _combat.Data)
            {
                if ((item.Contains("SPELL_DAMAGE") || item.Contains("SWING_DAMAGE")
                    || item.Contains("SPELL_PERIODIC_DAMAGE") || item.Contains("SWING_MISSED")
                    || item.Contains("SPELL_MISSED")) && item.Contains(_player))
                {
                    var combatData = CombatDataParse(item);
                    var damageTakenInformation = GetDamageTakenInformation(combatData.ToArray());

                    if (damageTakenInformation != null)
                    {
                        damageTaken += damageTakenInformation.Value;
                        DamageTakenInformations.Add(damageTakenInformation);
                    }
                }
            }

            return damageTaken;
        }

        public int GetDeathsNumber()
        {
            int deaths = 0;
            foreach (var item in _combat.Data)
            {
                if (item.Contains("UNIT_DIED"))
                {
                    deaths += CheckPlayer(item);
                }
            }

            return deaths;
        }

        private List<string> CombatDataParse(string combatData)
        {
            var log = combatData.Split("  ");
            var parse = log[1].Split(',');
            var time = log[0].Split(' ');

            var data = new List<string>
            {
                time[1]
            };

            data.AddRange(parse);

            return data;
        }

        private DamageDone GetDamageDoneInformation(string[] combatData)
        {
            if (!combatData[3].Contains(_player) 
                || combatData[1] == "SWING_DAMAGE_LANDED")
            {
                return null;
            }

            int.TryParse(combatData[^10], out var value1);

            var spellOrItem = (combatData[11].Contains("0000000000000000") || combatData[11].Contains("nil"))
                ? "Ближ. бой" : combatData[11].Trim('"');

            var isResist = false;
            var isImmune = false;
            var isParry = false;
            var isDodge = false;
            var isMiss = false;

            if (combatData[1] == "DAMAGE_SHIELD_MISSED")
            {
                isResist = combatData[13] == "RESIST" ? true : false;
                isImmune = combatData[13] == "IMMUNE" ? true : false;
            }
            else if (combatData[1] == "SPELL_MISSED")
            {
                isResist = combatData[13] == "RESIST" ? true : false;
                isParry = combatData[13] == "PARRY" ? true : false;
                isDodge = combatData[13] == "DODGE" ? true : false;
                isImmune = combatData[13] == "IMMUNE" ? true : false;
                isMiss = combatData[13] == "MISS" ? true : false;
            }
            else if (combatData[1] == "SWING_MISSED")
            {
                isResist = combatData[10] == "RESIST" ? true : false;
                isParry = combatData[10] == "PARRY" ? true : false;
                isDodge = combatData[10] == "DODGE" ? true : false;
                isImmune = combatData[10] == "IMMUNE" ? true : false;
                isMiss = combatData[10] == "MISS" ? true : false;
            }

            var isCrit = combatData[^3] == "1" ? true : false;

            var damageDone = new DamageDone
            {
                Value = value1,
                Time = TimeSpan.Parse(combatData[0]),
                FromPlayer = combatData[3].Trim('"'),
                ToEnemy = combatData[7].Trim('"'),
                SpellOrItem = spellOrItem,
                IsDodge = isDodge,
                IsMiss = isMiss,
                IsParry = isParry,
                IsResist = isResist,
                IsImmune = isImmune,
                IsCrit = isCrit,
            };

            return damageDone;
        }

        private EnergyRecovery GetEnergyInformation(string[] combatData)
        {
            int.TryParse(combatData[21], out var value1);
            int.TryParse(combatData[22], out var value2);
            int.TryParse(combatData[^1], out var value3);
            double.TryParse(combatData[29], NumberStyles.Number, CultureInfo.InvariantCulture, out var value4);

            var energyRecovery = new EnergyRecovery
            {
                Time = TimeSpan.Parse(combatData[0]),
                CurrentEnergy = value1,
                NowMaxEnergy = value2,
                MaxEnergy = value3,
                Value = value4,
            };

            return energyRecovery;
        }

        private HealDone GetHealDoneInformation(string[] combatData)
        {
            if (!combatData[3].Contains(_player))
            {
                return null;
            }

            int.TryParse(combatData[21], out var value1);
            int.TryParse(combatData[^1], out var value2);
            int.TryParse(combatData[^4], out var value3);
            int.TryParse(combatData[^3], out var value4);

            var isCrit = combatData[^1] == "1" ? true : false;

            var healDone = new HealDone
            {
                CurrentHealth = value1,
                Time = TimeSpan.Parse(combatData[0]),
                FromPlayer = combatData[3].Trim('"'),
                ToPlayer = combatData[7].Trim('"'),
                SpellOrItem = combatData[11].Trim('"'),
                MaxHealth = value2,
                ValueWithOverheal = value3,
                Overheal = value4,
                IsFullOverheal = value3 - value4 == 0,
                IsCrit = isCrit
            };

            return healDone;
        }

        private DamageTaken GetDamageTakenInformation(string[] combatData)
        {
            if (!combatData[2].Contains("Creature")
                || combatData[1] == "SWING_DAMAGE_LANDED")
            {
                return null;
            }

            int.TryParse(combatData[^10], out var value1);
            var spellOrItem = (combatData[11].Contains("0000000000000000") || combatData[11].Contains("nil"))
                ? "Ближ. бой" : combatData[11].Trim('"');

            var isResist = false;
            var isImmune = false;
            if (combatData[1] == "DAMAGE_SHIELD_MISSED")
            {
                isResist = combatData[13] == "RESIST" ? true : false;
                isImmune = combatData[13] == "IMMUNE" ? true : false;
            }

            var isCrushing = combatData[^1] == "1" ? true : false;

            var damageTaken = new DamageTaken
            {
                Value = value1,
                Time = TimeSpan.Parse(combatData[0]),
                From = combatData[3].Trim('"'),
                To = combatData[7].Trim('"'),
                SpellOrItem = spellOrItem,
                IsDodge = combatData[10] == "DODGE",
                IsParry = combatData[10] == "PARRY",
                IsMiss = combatData[10] == "MISS",
                IsResist = isResist,
                IsImmune = isImmune,
                IsCrushing = isCrushing,
            };

            return damageTaken;
        }

        private int CheckPlayer(string combatData)
        {
            var isFound = false;
            foreach (var item in _combat.Players)
            {
                if (combatData.Contains(item.UserName))
                {
                    isFound = true;
                    break;
                }
            }

            return isFound ? 1 : 0;
        }
    }
}
