﻿namespace CombatAnalysis.CommunicationDAL.Entities.Community;

public class Community
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int PolicyType { get; set; }

    public string AppUserId { get; set; }
}
