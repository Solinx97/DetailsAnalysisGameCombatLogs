using CombatAnalysis.WoW.CombatParser.Extensions;
using CombatAnalysis.WoW_12_1_0.CombatParser.Interfaces;
using CombatAnalysis.WoW_12_1_0.CombatParser.Services;
using MvvmCross.IoC;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Extensions;

public static class MvcIoCCollection
{
    public static void CombatParserDependencies(this IMvxIoCProvider provider)
    {
        provider.GeneralCombatParserDependencies();

        provider.RegisterType<ICombatParserService, CombatParserService>();
    }
}
