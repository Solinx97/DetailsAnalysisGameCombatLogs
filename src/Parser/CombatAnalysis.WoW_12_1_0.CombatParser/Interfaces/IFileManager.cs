namespace CombatAnalysis.WoW_12_1_0.CombatParser.Interfaces;

public interface IFileManager
{
    StreamReader StreamReader(string path);

    Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken);
}
