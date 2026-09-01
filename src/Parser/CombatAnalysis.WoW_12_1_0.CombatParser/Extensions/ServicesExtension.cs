using CombatAnalysis.WoW_12_1_0.CombatParser.Helpers;
using CombatAnalysis.WoW_12_1_0.CombatParser.Interfaces;
using CombatAnalysis.WoW_12_1_0.CombatParser.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Extensions;

public static class ServicesExtension
{
    public static void CombatParser(this IServiceCollection collection)
    {
        collection.AddScoped<ICombatParserService, CombatParserService>();
        collection.AddScoped<IHttpClientHelper, HttpClientHelper>();
    }
}
