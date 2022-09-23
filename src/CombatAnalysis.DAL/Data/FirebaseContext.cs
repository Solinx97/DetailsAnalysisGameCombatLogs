using Firebase.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CombatAnalysis.DAL.Data
{
    public class FirebaseContext : DbContext
    {
        public FirebaseContext(DbContextOptions<FirebaseContext> options, IConfiguration configuration) : base(options)
        {
            var databaseName = configuration.GetSection("Database:Name").Value;
            var connection = configuration.GetConnectionString(databaseName);

            FirebaseClient = new FirebaseClient(connection);
        }

        public FirebaseClient FirebaseClient { get; private set; }
    }
}
