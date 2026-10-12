//using CombatAnalysis.NotificationDAL.Data;
//using Microsoft.EntityFrameworkCore;

//namespace CombatAnalysis.UserDAL.IntegrationTests.RepositoryTests;

//public class RepositoryTestsBase
//{
//    protected static NotificationContext CreateInMemoryContext(string dbName)
//    {
//        var options = new DbContextOptionsBuilder<NotificationContext>()
//            .UseInMemoryDatabase(databaseName: dbName + Guid.NewGuid().ToString())
//            .Options;

//        return new NotificationContext(options);
//    }
//}
