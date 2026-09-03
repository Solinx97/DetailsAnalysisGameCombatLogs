using CombatAnalysis.WoW.CombatParser.Helpers;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using MvvmCross.IoC;

namespace CombatAnalysis.WoW.CombatParser.Extensions;

public static class MvcIoCCollection
{
    public static void GeneralCombatParserDependencies(this IMvxIoCProvider provider)
    {
        provider.RegisterType<IHttpClientHelper, HttpClientHelper>();
    }
}
