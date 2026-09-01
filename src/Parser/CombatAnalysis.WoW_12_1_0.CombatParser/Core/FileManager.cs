using CombatAnalysis.WoW_12_1_0.CombatParser.Interfaces;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Core;

public class FileManager : IFileManager
{
    public StreamReader StreamReader(string path)
        => new(path);

    public Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken)
        => File.ReadAllLinesAsync(path, cancellationToken);
}
