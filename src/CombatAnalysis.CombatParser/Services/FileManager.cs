using CombatAnalysis.CombatParser.Interfaces;
using System.IO;

namespace CombatAnalysis.CombatParser.Services
{
    public class FileManager : IFileManager
    {
        public StreamReader StreamReader(string path)
        {
            return new StreamReader(path);
        }
    }
}
