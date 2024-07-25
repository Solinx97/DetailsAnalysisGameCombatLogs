using Firebase.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CombatAnalysis.ChatDAL.Data;

public class FirebaseContext : DbContext
{
    public FirebaseContext(DbContextOptions<FirebaseContext> options, IConfiguration configuration) : base(options)
    {
        var databaseName = configuration.GetSection("Database:Name").Value ?? string.Empty;
        var connection = configuration.GetConnectionString(databaseName);

        FirebaseClient = new FirebaseClient(connection);
    }

    public FirebaseClient FirebaseClient { get; private set; }
}
