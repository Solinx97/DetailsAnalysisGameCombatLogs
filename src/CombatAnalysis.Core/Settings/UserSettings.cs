﻿using System.Text.Json.Serialization;

namespace CombatAnalysis.Core.Settings;

public class UserSettings
{
    [JsonPropertyName("location")]
    public string Location { get; set; }

    public UserSettings(string location)
    {
        Location = location;
    }
}
