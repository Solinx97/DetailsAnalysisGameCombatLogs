using System;

namespace CombatAnalysis.DAL.Entities
{
    public class CombatLog
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset Date { get; set; }
    }
}