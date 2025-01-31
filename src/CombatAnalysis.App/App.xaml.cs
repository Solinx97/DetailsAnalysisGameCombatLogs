using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Views;
using System.Windows;

namespace CombatAnalysis.App;

public partial class App : MvxApplication
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        AppCenter.Start("f55e7c1e-17d9-4bf3-b2a8-cdb2d43128e7",
            typeof(Analytics), typeof(Crashes));
    }

    protected override void RegisterSetup()
    {
        base.RegisterSetup();
        this.RegisterSetupType<Setup>();
    }
}
