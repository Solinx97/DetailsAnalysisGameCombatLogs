using CombatAnalysis.EnhancedWebApp.Server.Enums;
using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account.Collections;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(WoWAccountCollectionItemInfoDto), (int)WoWAccountCollectionType.MOUNT)]
[JsonDerivedType(typeof(WoWAccountPetInfoDto), (int)WoWAccountCollectionType.PET)]
public class WoWAccountCollectionItemInfoDto
{
    public bool IsFavorite { get; set; }
}
