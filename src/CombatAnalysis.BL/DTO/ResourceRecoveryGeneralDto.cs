namespace CombatAnalysis.BL.DTO
{
    public class ResourceRecoveryGeneralDto
    {
        public int Value { get; set; }

        public double ResourcePerSecond { get; set; }

        public string SpellOrItem { get; set; }

        public int CastNumber { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public double AverageValue { get; set; }

        public int CombatPlayerDataId { get; set; }
    }
}
