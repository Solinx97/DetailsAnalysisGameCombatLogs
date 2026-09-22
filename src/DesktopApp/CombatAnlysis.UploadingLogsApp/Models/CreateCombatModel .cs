namespace CombatAnalysis.UploadingLogsApp.Models;

public class CreateCombatModel : CombatModel
{
    public bool IsSelected { get; set; }

    public bool IsSupported { get; set; }

    public int GameVersion { get; set; }
}
