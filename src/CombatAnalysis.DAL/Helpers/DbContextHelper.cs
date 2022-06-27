using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Helpers
{
    public static class DbProcedureHelper
    {
        public static string GetCombat = "GetCombatByCombatLogId";
        public static string GetCombatPlayer = "GetCombatPlayerByCombatId";
        public static string GetDamageDone = "GetDamageDoneByCombatPlayerId";
        public static string GetDamageDoneGeneric = "GetDamageDoneGenericByCombatPlayerId";
        public static string GetHealDone = "GetHealDoneByCombatPlayerId";
        public static string GetHealDoneGeneric = "GetHealDoneGenericByCombatPlayerId";
        public static string GetDamageTaken = "GetDamageTakenByCombatPlayerId";
        public static string GetDamageTakenGeneric = "GetDamageTakenGenericByCombatPlayerId";
        public static string GetResourceRecovery = "GetResourceRecoveryByCombatPlayerId";
        public static string GetResourceRecoveryGeneric = "GetResourceRecoveryGenericByCombatPlayerId";

        public static async Task CreateProceduresAsync(DbContext dbContext)
        {
            var query = @"CREATE PROCEDURE GetCombatByCombatLogId (@combatLogId INT)
                          AS SELECT *
                          FROM Combat
                          WHERE CombatLogId = @combatLogId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetCombatPlayerByCombatId (@combatId INT)
                          AS SELECT *
                          FROM CombatPlayerData
                          WHERE CombatId = @combatId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetDamageDoneByCombatPlayerId (@combatPlayerDataId INT)
                          AS SELECT *
                          FROM DamageDone
                          WHERE CombatPlayerDataId = @combatPlayerDataId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetDamageDoneGenericByCombatPlayerId (@combatPlayerDataId INT)
                          AS SELECT *
                          FROM DamageDoneGeneric
                          WHERE CombatPlayerDataId = @combatPlayerDataId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetHealDoneByCombatPlayerId (@combatPlayerDataId INT)
                          AS SELECT *
                          FROM HealDone
                          WHERE CombatPlayerDataId = @combatPlayerDataId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetHealDoneGenericByCombatPlayerId (@combatPlayerDataId INT)
                          AS SELECT *
                          FROM HealDoneGeneric
                          WHERE CombatPlayerDataId = @combatPlayerDataId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetDamageTakenByCombatPlayerId (@combatPlayerDataId INT)
                          AS SELECT *
                          FROM DamageTaken
                          WHERE CombatPlayerDataId = @combatPlayerDataId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetDamageTakenGenericByCombatPlayerId (@combatPlayerDataId INT)
                          AS SELECT *
                          FROM DamageTakenGeneric
                          WHERE CombatPlayerDataId = @combatPlayerDataId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetResourceRecoveryByCombatPlayerId (@combatPlayerDataId INT)
                          AS SELECT *
                          FROM ResourceRecovery
                          WHERE CombatPlayerDataId = @combatPlayerDataId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetResourceRecoveryGenericByCombatPlayerId (@combatPlayerDataId INT)
                          AS SELECT *
                          FROM ResourceRecoveryGeneric
                          WHERE CombatPlayerDataId = @combatPlayerDataId";
            await dbContext.Database.ExecuteSqlRawAsync(query);
        }
    }
}
