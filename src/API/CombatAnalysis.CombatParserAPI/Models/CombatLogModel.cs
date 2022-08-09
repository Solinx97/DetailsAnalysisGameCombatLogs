using System;

namespace CombatAnalysis.CombatParserAPI.Models
{
    public class CombatLogModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset Date { get; set; }
    }
}
