using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Entities
{
    [Index(nameof(CombatPlayerDataId))]
    public class ResourceRecovery
    {
        public int Id { get; set; }

        public double Value { get; set; }

        public string Time { get; set; }

        public string SpellOrItem { get; set; }

        public int CombatPlayerDataId { get; set; }
    }
}
