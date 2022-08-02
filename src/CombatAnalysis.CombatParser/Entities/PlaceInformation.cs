using System;

namespace CombatAnalysis.CombatParser.Entities
{
    public class PlaceInformation
    {
        public string Name { get; set; }

        public DateTimeOffset EntryDate { get; set; }

        public int PlaceType { get; set; }
    }
}