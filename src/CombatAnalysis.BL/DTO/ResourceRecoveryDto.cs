namespace CombatAnalysis.BL.DTO
{
    public class ResourceRecoveryDto
    {
        public int Id { get; set; }

        public double Value { get; set; }

        public string Time { get; set; }

        public string SpellOrItem { get; set; }

        public int CombatPlayerDataId { get; set; }
    }
}
