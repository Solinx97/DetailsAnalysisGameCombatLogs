﻿using CombatAnalysis.CombatParser.Entities;
using System.Collections.Generic;

namespace CombatAnalysis.CombatParser.Patterns
{
    public abstract class CombatDetailsTemplate
    {
        public List<DamageDone> DamageDone { get; protected set; }

        public List<HealDone> HealDone { get; protected set; }

        public List<DamageTaken> DamageTaken { get; protected set; }

        public List<ResourceRecovery> ResourceRecovery { get; protected set; }

        public abstract int GetData(string player, List<string> combatData);

        protected List<string> GetUsefulInformation(string combatData)
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
    }
}