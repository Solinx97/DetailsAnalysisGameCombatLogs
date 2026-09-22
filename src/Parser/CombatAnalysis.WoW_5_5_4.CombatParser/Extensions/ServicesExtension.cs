using CombatAnalysis.WoW.CombatParser.Extensions;
using CombatAnalysis.WoW_5_5_4.CombatParser.Interfaces;
using CombatAnalysis.WoW_5_5_4.CombatParser.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.WoW_5_5_4.CombatParser.Extensions;

public static class ServicesExtension
{
    public static void CombatParserWoW_5_5_4(this IServiceCollection collection)
    {
        collection.GeneralCombatParser();

        collection.AddScoped<ICombatParserService, CombatParserService>();
    }
}
