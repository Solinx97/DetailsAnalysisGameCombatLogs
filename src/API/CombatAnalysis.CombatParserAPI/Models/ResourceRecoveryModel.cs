namespace CombatAnalysis.CombatParserAPI.Models
{
    public class ResourceRecoveryModel
    {
        public double Value { get; set; }

        public string Time { get; set; }

        public string SpellOrItem { get; set; }

        public int CombatPlayerDataId { get; set; }
    }
}
