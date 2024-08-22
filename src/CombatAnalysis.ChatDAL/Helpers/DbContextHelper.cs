using CombatAnalysis.ChatDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatDAL.Helpers;

internal static class DbContextHelper
{
    public static void CreateProcedures(DbContext dbContext)
    {
        var types = new Type[]
        {
            typeof(PersonalChatMessage),
            typeof(GroupChatMessage),
        };

        foreach (var item in types)
        {
            var query = $"CREATE PROCEDURE Get{item.Name}ByChatIdPagination (@chatId INT, @pageSize INT)\n" +
                          "\tAS\n" +
                          "\tSELECT TOP (@pageSize) * \n" +
                         $"\tFROM {item.Name}\n" +
                          "\tWHERE ChatId = @chatId\n" +
                         $"\tORDER BY Id\n DESC";
            dbContext.Database.ExecuteSqlRaw(query);
        }
    }
}
