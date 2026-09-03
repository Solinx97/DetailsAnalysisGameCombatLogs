using CombatAnalysis.WoW.CombatParser.Core;
using CombatAnalysis.WoW.CombatParser.Helpers;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CombatAnalysis.WoW.CombatParser.Extensions;

public static class ServicesExtension
{
    public static void GeneralCombatParser(this IServiceCollection collection)
    {
        collection.AddScoped<IHttpClientHelper, HttpClientHelper>();
        collection.AddScoped<IFileManager, FileManager>();
    }
}
