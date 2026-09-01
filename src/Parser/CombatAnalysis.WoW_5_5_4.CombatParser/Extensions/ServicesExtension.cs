using CombatAnalysis.WoW_5_5_4.CombatParser.Helpers;
using CombatAnalysis.WoW_5_5_4.CombatParser.Interfaces;
using CombatAnalysis.WoW_5_5_4.CombatParser.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.WoW_5_5_4.CombatParser.Extensions;

public static class ServicesExtension
{
    public static void CombatParser(this IServiceCollection collection)
    {
        collection.AddScoped<ICombatParserService, CombatParserService>();
        collection.AddScoped<IHttpClientHelper, HttpClientHelper>();
    }
}
