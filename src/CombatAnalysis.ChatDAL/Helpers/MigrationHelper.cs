using CombatAnalysis.ChatDAL.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CombatAnalysis.ChatDAL.Helpers;

internal static class MigrationHelper
{
    private static readonly string[] _procedureNames =
    [
        $"Get{nameof(PersonalChatMessage)}ByChatIdPagination",
        $"Get{nameof(PersonalChatMessage)}ByChatIdMore",
        $"Get{nameof(GroupChatMessage)}ByChatIdPagination",
        $"Get{nameof(GroupChatMessage)}ByChatIdMore",
    ];

    public static void CreateProcedures(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[0]} (@chatId INT, @pageSize INT)\n" +
                              "AS\n" +
                              "BEGIN\n" +
                              "\tSELECT TOP (@pageSize) * \n" +
                             $"\tFROM {nameof(PersonalChatMessage)}\n" +
                              "\tWHERE ChatId = @chatId\n" +
                              "\tORDER BY Id DESC\n" +
                              "END");

        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[1]} (@chatId INT, @offset INT, @pageSize INT)\n" +
                             "AS\n" +
                             "BEGIN\n" +
                             "\tSELECT * \n" +
                            $"\tFROM {nameof(PersonalChatMessage)}\n" +
                             "\tWHERE ChatId = @chatId\n" +
                             "\tORDER BY Id DESC\n" +
                             "\tOFFSET @offset ROWS\n" +
                             "\tFETCH NEXT @pageSize ROWS ONLY\n" +
                             "END");

        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[2]} (@chatId INT, @pageSize INT)\n" +
                              "AS\n" +
                              "BEGIN\n" +
                              "\tSELECT TOP (@pageSize) * \n" +
                             $"\tFROM {nameof(GroupChatMessage)}\n" +
                              "\tWHERE ChatId = @chatId\n" +
                              "\tORDER BY Id DESC\n" +
                              "END");

        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[3]} (@chatId INT, @offset INT, @pageSize INT)\n" +
                             "AS\n" +
                             "BEGIN\n" +
                             "\tSELECT * \n" +
                            $"\tFROM {nameof(GroupChatMessage)}\n" +
                             "\tWHERE ChatId = @chatId\n" +
                             "\tORDER BY Id DESC\n" +
                             "\tOFFSET @offset ROWS\n" +
                             "\tFETCH NEXT @pageSize ROWS ONLY\n" +
                             "END");
    }

    public static void DropProcedures(MigrationBuilder migrationBuilder)
    {
        foreach (var procedureName in _procedureNames)
        {
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS {procedureName}");
        }
    }
}
