using Microsoft.Win32;
using System;

namespace CombatAnalysis.WinCore
{
    public static class WinHandler
    {
        public static string FileOpen()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            fileDialog.ShowDialog();

            return fileDialog.FileName;
        }
    }
}