using CombatAnalysis.WoW.CombatParser.Extensions;
using CombatAnalysis.WoW_5_5_4.CombatParser.Interfaces;
using CombatAnalysis.WoW_5_5_4.CombatParser.Services;
using MvvmCross.IoC;

namespace CombatAnalysis.WoW_5_5_4.CombatParser.Extensions;

public static class MvcIoCCollection
{
    public static void CombatParserDependencies(this IMvxIoCProvider provider)
    {
        provider.GeneralCombatParserDependencies();

        provider.RegisterType<ICombatParserService, CombatParserService>();
    }
}
