using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Helpers
{
    public static class DbProcedureHelper
    {
        public static string GetCombat = "GetCombatByCombatLogId";
        public static string GetCombatPlayer = "GetCombatPlayerByCombatId";
        public static string GetDamageDone = "GetDamageDoneByCombatPlayerId";
        public static string GetDamageDoneGeneral = "GetDamageDoneGeneralByCombatPlayerId";
        public static string GetHealDone = "GetHealDoneByCombatPlayerId";
        public static string GetHealDoneGeneral = "GetHealDoneGeneralByCombatPlayerId";
        public static string GetDamageTaken = "GetDamageTakenByCombatPlayerId";
        public static string GetDamageTakenGeneral = "GetDamageTakenGeneralByCombatPlayerId";
        public static string GetResourceRecovery = "GetResourceRecoveryByCombatPlayerId";
        public static string GetResourceRecoveryGeneral = "GetResourceRecoveryGeneralByCombatPlayerId";

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

            query = @"CREATE PROCEDURE GetDamageDoneGeneralByCombatPlayerId (@combatPlayerDataId INT)
                          AS SELECT *
                          FROM DamageDoneGeneral
                          WHERE CombatPlayerDataId = @combatPlayerDataId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetHealDoneByCombatPlayerId (@combatPlayerDataId INT)
                          AS SELECT *
                          FROM HealDone
                          WHERE CombatPlayerDataId = @combatPlayerDataId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetHealDoneGeneralByCombatPlayerId (@combatPlayerDataId INT)
                          AS SELECT *
                          FROM HealDoneGeneral
                          WHERE CombatPlayerDataId = @combatPlayerDataId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetDamageTakenByCombatPlayerId (@combatPlayerDataId INT)
                          AS SELECT *
                          FROM DamageTaken
                          WHERE CombatPlayerDataId = @combatPlayerDataId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetDamageTakenGeneralByCombatPlayerId (@combatPlayerDataId INT)
                          AS SELECT *
                          FROM DamageTakenGeneral
                          WHERE CombatPlayerDataId = @combatPlayerDataId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetResourceRecoveryByCombatPlayerId (@combatPlayerDataId INT)
                          AS SELECT *
                          FROM ResourceRecovery
                          WHERE CombatPlayerDataId = @combatPlayerDataId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetResourceRecoveryGeneralByCombatPlayerId (@combatPlayerDataId INT)
                          AS SELECT *
                          FROM ResourceRecoveryGeneral
                          WHERE CombatPlayerDataId = @combatPlayerDataId";
            await dbContext.Database.ExecuteSqlRawAsync(query);
        }
    }
}
