using CombatAnalysis.CombatParser.Entities;
using System.Collections.Generic;

namespace CombatAnalysis.CombatParser.Interfaces
{
    public interface ICombatDetails
    {
        List<DamageDone> DamageDone { get; }

        List<HealDone> HealDone { get; }

        List<DamageTaken> DamageTaken { get; }

        List<ResourceRecovery> ResourceRecovery { get; }

        void SetData(Combat combat, string player);

        void SetData(Combat combat);

        void SetData(string player);

        int GetDamageDone();

        int GetHealDone();

        int GetDamageTaken();

        double GetResourceRecovery();

        int GetDeathsNumber();
    }
}
