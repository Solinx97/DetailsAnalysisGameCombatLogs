namespace CombatAnalysis.StoredProcedureBL.DTO
{
    public class ResourceRecoveryDto
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public string Time { get; set; }

        public string SpellOrItem { get; set; }

        public int CombatPlayerId { get; set; }
    }
}
