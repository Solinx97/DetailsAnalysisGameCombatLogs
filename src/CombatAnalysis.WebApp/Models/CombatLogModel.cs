using System;

namespace CombatAnalysis.WebApp.Models
{
    public class CombatLogModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset Date { get; set; }

        public bool IsReady { get; set; }
    }
}
