using CombatAnalysis.CombatParser.Helpers;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.CombatParser.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.CombatParser.Extensions;

public static class ServicesExtension
{
    public static void CombatParser(this IServiceCollection collection)
    {
        collection.AddScoped<ICombatParserService, CombatParserService>();
        collection.AddScoped<IHttpClientHelper, HttpClientHelper>();
    }
}
