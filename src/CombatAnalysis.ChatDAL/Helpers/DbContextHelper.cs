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
                          "\tORDER BY Id DESC";
            dbContext.Database.ExecuteSqlRaw(query);

            query = $"CREATE PROCEDURE Get{item.Name}ByChatIdMore (@chatId INT, @offset INT, @pageSize INT)\n" +
                     "\tAS\n" +
                     "\tSELECT * \n" +
                    $"\tFROM {item.Name}\n" +
                     "\tWHERE ChatId = @chatId\n" +
                     "\tORDER BY Id DESC\n" +
                     "\tOFFSET @offset ROWS\n" +
                     "\tFETCH NEXT @pageSize ROWS ONLY";
            dbContext.Database.ExecuteSqlRaw(query);
        }
    }
}
