using Firebase.Database;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Data
{
    public class FirebaseContext : DbContext
    {
        private const string FirebaseDatabaseUrl = "https://detailsanalysisgamecombatlogs-default-rtdb.firebaseio.com/";

        public FirebaseContext()
        {
            FirebaseClient = new FirebaseClient(FirebaseDatabaseUrl);
        }

        public FirebaseClient FirebaseClient { get; private set; }
    }
}
