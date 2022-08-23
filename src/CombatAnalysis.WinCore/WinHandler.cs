using Microsoft.Win32;

namespace CombatAnalysis.WinCore
{
    public static class WinHandler
    {
        public static string FileOpen()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();

            return fileDialog.FileName;
        }
    }
}