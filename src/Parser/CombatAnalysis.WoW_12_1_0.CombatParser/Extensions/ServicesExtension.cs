using CombatAnalysis.WoW.CombatParser.Extensions;
using CombatAnalysis.WoW_12_1_0.CombatParser.Interfaces;
using CombatAnalysis.WoW_12_1_0.CombatParser.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Extensions;

public static class ServicesExtension
{
    public static void CombatParserWoW_12_1_0(this IServiceCollection collection)
    {
        collection.GeneralCombatParser();

        collection.AddScoped<ICombatParserService, CombatParserService>();
    }
}
