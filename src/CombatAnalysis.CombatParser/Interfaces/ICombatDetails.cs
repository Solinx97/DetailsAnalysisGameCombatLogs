using CombatAnalysis.CombatParser.Entities;

namespace CombatAnalysis.CombatParser.Interfaces;

public interface ICombatDetails
{
    List<DamageDone> DamageDone { get; }

    List<HealDone> HealDone { get; }

    List<DamageTaken> DamageTaken { get; }

    List<ResourceRecovery> ResourceRecovery { get; }

    void Initialization(Combat combat, string player);

    void Initialization(Combat combat);

    void Initialization(string player);

    void Clear();

    //int GetDamageDone();

    int GetHealDone();

    int GetDamageTaken();

    double GetResourceRecovery();

    int GetDeathsNumber();
}
