using CombatAnalysis.CommunicationDAL.Entities.Post;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CommunicationDAL.Helpers;

internal static class DbContextHelper
{
    public static void CreateProcedures(DbContext dbContext)
    {
        var query = $"CREATE PROCEDURE Get{nameof(CommunityPost)}ByCommunityIdPagination (@communityId INT, @pageSize INT)\n" +
                      "\tAS\n" +
                      "\tSELECT TOP (@pageSize) * \n" +
                     $"\tFROM {nameof(CommunityPost)}\n" +
                      "\tWHERE CommunityId = @communityId\n" +
                      "\tORDER BY Id DESC";
        dbContext.Database.ExecuteSqlRaw(query);

        query = $"CREATE PROCEDURE GetMore{nameof(CommunityPost)}ByCommunityId (@communityId INT, @offset INT, @pageSize INT)\n" +
                 "\tAS\n" +
                 "\tSELECT * \n" +
                $"\tFROM {nameof(CommunityPost)}\n" +
                 "\tWHERE CommunityId = @communityId\n" +
                 "\tORDER BY Id DESC\n" +
                 "\tOFFSET @offset ROWS\n" +
                 "\tFETCH NEXT @pageSize ROWS ONLY";
        dbContext.Database.ExecuteSqlRaw(query);

        query = $"CREATE PROCEDURE GetNew{nameof(CommunityPost)}ByCommunityId (@communityId INT, @checkFrom DATETIME)\n" +
                      "\tAS\n" +
                      "\tSELECT * \n" +
                     $"\tFROM {nameof(CommunityPost)}\n" +
                      "\tWHERE CommunityId = @communityId AND CreatedAt > @checkFrom\n" +
                      "\tORDER BY CreatedAt DESC";
        dbContext.Database.ExecuteSqlRaw(query);

        query = $"CREATE PROCEDURE Get{nameof(UserPost)}ByAppUserIdPagination (@appUserId NVARCHAR (MAX), @pageSize INT)\n" +
                      "\tAS\n" +
                      "\tSELECT TOP (@pageSize) * \n" +
                     $"\tFROM {nameof(UserPost)}\n" +
                      "\tWHERE AppUserId = @appUserId\n" +
                      "\tORDER BY Id DESC";
        dbContext.Database.ExecuteSqlRaw(query);

        query = $"CREATE PROCEDURE GetMore{nameof(UserPost)}ByAppUserId (@appUserId NVARCHAR (MAX), @offset INT, @pageSize INT)\n" +
                 "\tAS\n" +
                 "\tSELECT * \n" +
                $"\tFROM {nameof(UserPost)}\n" +
                 "\tWHERE AppUserId = @appUserId\n" +
                 "\tORDER BY Id DESC\n" +
                 "\tOFFSET @offset ROWS\n" +
                 "\tFETCH NEXT @pageSize ROWS ONLY";
        dbContext.Database.ExecuteSqlRaw(query);

        query = $"CREATE PROCEDURE GetNew{nameof(UserPost)}ByAppUserId (@appUserId NVARCHAR (MAX), @checkFrom DATETIME)\n" +
              "\tAS\n" +
              "\tSELECT * \n" +
             $"\tFROM {nameof(UserPost)}\n" +
              "\tWHERE AppUserId = @appUserId AND CreatedAt > @checkFrom\n" +
              "\tORDER BY CreatedAt DESC";
        dbContext.Database.ExecuteSqlRaw(query);
    }
}
