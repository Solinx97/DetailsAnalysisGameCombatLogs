﻿namespace CombatAnalysis.CustomerBL.DTO;

public class RequestToConnectDto
{
    public int Id { get; set; }

    public string ToUserId { get; set; }

    public DateTimeOffset When { get; set; }

    public int Result { get; set; }

    public string OwnerId { get; set; }
}