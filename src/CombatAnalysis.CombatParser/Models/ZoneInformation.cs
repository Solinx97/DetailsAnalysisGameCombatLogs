using System;

namespace CombatAnalysis.CombatParser.Models
{
    public class ZoneInformation
    {
        public string Name { get; set; }

        public DateTimeOffset ChangeDate { get; set; }

        public int ZoneType { get; set; }
    }
}