namespace CombatAnalysis.CustomerDAL.Entities.Authentication;

public class Secret
{
    public int Id { get; set; }

    public string AccessSecret { get; set; }

    public string RefreshSecret { get; set; }
}
