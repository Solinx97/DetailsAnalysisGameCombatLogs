using CombatAnalysis.CommunicationDAL.Entities.Post;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CombatAnalysis.CommunicationDAL.Helpers;

internal static class MigrationHelper
{
    private static readonly string[] _procedureNames =
    [
        $"Get{nameof(CommunityPost)}ByCommunityIdPagination",
        $"GetMore{nameof(CommunityPost)}ByCommunityId",
        $"GetNew{nameof(CommunityPost)}ByCommunityId",
        $"Get{nameof(CommunityPost)}ByListOfCommunityIdPagination",
        $"GetMore{nameof(CommunityPost)}ByListOfCommunityId",
        $"GetNew{nameof(CommunityPost)}ByListOfCommunityId",
        $"Get{nameof(UserPost)}ByAppUserIdPagination",
        $"GetMore{nameof(UserPost)}ByAppUserId",
        $"GetNew{nameof(UserPost)}ByAppUserId",
        $"Get{nameof(UserPost)}ByListOfAppUserIdPagination",
        $"GetMore{nameof(UserPost)}ByListOfAppUserId",
        $"GetNew{nameof(UserPost)}ByListOfAppUserId",
    ];

    public static void CreateProcedures(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[0]} (@communityId INT, @pageSize INT)\n" +
                      "AS\n" +
                      "BEGIN\n" +
                      "\tSELECT TOP (@pageSize) * \n" +
                     $"\tFROM {nameof(CommunityPost)}\n" +
                      "\tWHERE CommunityId = @communityId\n" +
                      "\tORDER BY Id DESC\n" +
                      "END");

        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[1]} (@communityId INT, @offset INT, @pageSize INT)\n" +
                 "AS\n" +
                 "BEGIN\n" +
                 "\tSELECT * \n" +
                $"\tFROM {nameof(CommunityPost)}\n" +
                 "\tWHERE CommunityId = @communityId\n" +
                 "\tORDER BY Id DESC\n" +
                 "\tOFFSET @offset ROWS\n" +
                 "\tFETCH NEXT @pageSize ROWS ONLY\n" +
                 "END");

        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[2]} (@communityId INT, @checkFrom DATETIME)\n" +
                 "AS\n" +
                 "BEGIN\n" +
                 "\tSELECT * \n" +
                $"\tFROM {nameof(CommunityPost)}\n" +
                 "\tWHERE CommunityId = @communityId AND CreatedAt > @checkFrom\n" +
                 "\tORDER BY CreatedAt DESC\n" +
                 "END");

        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[3]} (@communityIds NVARCHAR(MAX), @pageSize INT)\n" +
                  "AS\n" +
                  "BEGIN\n" +
                  "\tDECLARE @communityIdTable TABLE (CommunityId INT);\n" +
                  "\tINSERT INTO @communityIdTable (CommunityId)\n" +
                  "\tSELECT value FROM STRING_SPLIT(@communityIds, ',');\n" +
                  "\tSELECT TOP (@pageSize) * \n" +
                 $"\tFROM {nameof(CommunityPost)}\n" +
                  "\tWHERE CommunityId IN (SELECT CommunityId FROM @communityIdTable)\n" +
                  "\tORDER BY Id DESC\n" +
                  "END");

        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[4]} (@communityIds NVARCHAR(MAX), @offset INT, @pageSize INT)\n" +
                  "AS\n" +
                  "BEGIN\n" +
                  "\tDECLARE @communityIdTable TABLE (CommunityId INT);\n" +
                  "\tINSERT INTO @communityIdTable (CommunityId)\n" +
                  "\tSELECT value FROM STRING_SPLIT(@communityIds, ',');\n" +
                  "\tSELECT * \n" +
                 $"\tFROM {nameof(CommunityPost)}\n" +
                  "\tWHERE CommunityId IN (SELECT CommunityId FROM @communityIdTable)\n" +
                  "\tORDER BY Id DESC\n" +
                  "\tOFFSET @offset ROWS\n" +
                  "\tFETCH NEXT @pageSize ROWS ONLY\n" +
                  "END");

        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[5]} (@communityIds NVARCHAR(MAX), @checkFrom DATETIME)\n" +
                  "AS\n" +
                  "BEGIN\n" +
                  "\tDECLARE @communityIdTable TABLE (CommunityId INT);\n" +
                  "\tINSERT INTO @communityIdTable (CommunityId)\n" +
                  "\tSELECT value FROM STRING_SPLIT(@communityIds, ',');\n" +
                  "\tSELECT * \n" +
                 $"\tFROM {nameof(CommunityPost)}\n" +
                  "\tWHERE CommunityId IN (SELECT CommunityId FROM @communityIdTable) AND CreatedAt > @checkFrom\n" +
                  "\tORDER BY CreatedAt DESC\n" +
                  "END");

        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[6]} (@appUserId NVARCHAR (MAX), @pageSize INT)\n" +
                  "AS\n" +
                  "BEGIN\n" +
                  "\tSELECT TOP (@pageSize) * \n" +
                 $"\tFROM {nameof(UserPost)}\n" +
                  "\tWHERE AppUserId = @appUserId\n" +
                  "\tORDER BY Id DESC\n" +
                  "END");

        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[7]} (@appUserId NVARCHAR (MAX), @offset INT, @pageSize INT)\n" +
                 "AS\n" +
                 "BEGIN\n" +
                 "\tSELECT * \n" +
                $"\tFROM {nameof(UserPost)}\n" +
                 "\tWHERE AppUserId = @appUserId\n" +
                 "\tORDER BY Id DESC\n" +
                 "\tOFFSET @offset ROWS\n" +
                 "\tFETCH NEXT @pageSize ROWS ONLY\n" +
                 "END");

        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[8]} (@appUserId NVARCHAR (MAX), @checkFrom DATETIME)\n" +
                 "AS\n" +
                 "BEGIN\n" +
                 "\tSELECT * \n" +
                $"\tFROM {nameof(UserPost)}\n" +
                 "\tWHERE AppUserId = @appUserId AND CreatedAt > @checkFrom\n" +
                 "\tORDER BY CreatedAt DESC\n" +
                 "END");

        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[9]} (@appUserIds NVARCHAR (MAX), @pageSize INT)\n" +
                  "AS\n" +
                  "BEGIN\n" +
                  "\tDECLARE @appUserIdTable TABLE (AppUserId NVARCHAR (MAX));\n" +
                  "\tINSERT INTO @appUserIdTable (AppUserId)\n" +
                  "\tSELECT value FROM STRING_SPLIT(@appUserIds, ',');\n" +
                  "\tSELECT TOP (@pageSize) * \n" +
                 $"\tFROM {nameof(UserPost)}\n" +
                  "\tWHERE AppUserId IN (SELECT AppUserId FROM @appUserIdTable)\n" +
                  "\tORDER BY Id DESC\n" +
                  "END");

        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[10]} (@appUserIds NVARCHAR (MAX), @offset INT, @pageSize INT)\n" +
                  "AS\n" +
                  "BEGIN\n" +
                  "\tDECLARE @appUserIdTable TABLE (AppUserId NVARCHAR (MAX));\n" +
                  "\tINSERT INTO @appUserIdTable (AppUserId)\n" +
                  "\tSELECT value FROM STRING_SPLIT(@appUserIds, ',');\n" +
                  "\tSELECT * \n" +
                 $"\tFROM {nameof(UserPost)}\n" +
                  "\tWHERE AppUserId IN (SELECT AppUserId FROM @appUserIdTable)\n" +
                  "\tORDER BY Id DESC\n" +
                  "\tOFFSET @offset ROWS\n" +
                  "\tFETCH NEXT @pageSize ROWS ONLY\n" +
                  "END");

        migrationBuilder.Sql($"CREATE PROCEDURE {_procedureNames[11]} (@appUserIds NVARCHAR (MAX), @checkFrom DATETIME)\n" +
                  "AS\n" +
                  "BEGIN\n" +
                  "\tDECLARE @appUserIdTable TABLE (AppUserId NVARCHAR (MAX));\n" +
                  "\tINSERT INTO @appUserIdTable (AppUserId)\n" +
                  "\tSELECT value FROM STRING_SPLIT(@appUserIds, ',');\n" +
                  "\tSELECT * \n" +
                 $"\tFROM {nameof(UserPost)}\n" +
                  "\tWHERE AppUserId IN (SELECT AppUserId FROM @appUserIdTable) AND CreatedAt > @checkFrom\n" +
                  "\tORDER BY CreatedAt DESC\n" +
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
