namespace CombatAnalysis.StoredProcedureDAL.Entities
{
    public class CombatPlayer
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public int EnergyRecovery { get; set; }

        public int DamageDone { get; set; }

        public int HealDone { get; set; }

        public int DamageTaken { get; set; }

        public int UsedBuffs { get; set; }

        public int CombatId { get; set; }
    }
}
