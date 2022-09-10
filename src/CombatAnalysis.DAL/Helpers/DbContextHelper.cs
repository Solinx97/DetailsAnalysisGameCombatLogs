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
        public static string InsertIntoCombat = "InsertIntoCombat";
        public static string InsertIntoCombatPlayer = "InsertIntoCombatPlayer";
        public static string InsertIntoDamageDone = "InsertIntoDamageDone";
        public static string InsertIntoDamageDoneGeneral = "InsertIntoDamageDoneGeneral";
        public static string InsertIntoHealDone = "InsertIntoHealDone";
        public static string InsertIntoHealDoneGeneral = "InsertIntoHealDoneGeneral";
        public static string InsertIntoDamageTaken = "InsertIntoDamageTaken";
        public static string InsertIntoDamageTakenGeneral = "InsertIntoDamageTakenGeneral";
        public static string InsertIntoResourceRecovery = "InsertIntoResourceRecovery";
        public static string InsertIntoResourceRecoveryGeneral = "InsertIntoResourceRecoveryGeneral";
        public static string DeleteCombat = "DeleteCombatByCombatLogId";
        public static string DeleteCombatPlayer = "DeleteCombatPlayerByCombatId";
        public static string DeleteHealDone = "DeleteHealDoneByCombatPlayerId";
        public static string DeleteHealDoneGeneral = "DeleteHealDoneGeneralByCombatPlayerId";
        public static string DeleteDamageDone = "DeleteDamageDoneByCombatPlayerId";
        public static string DeleteDamageDoneGeneral = "DeleteDamageDoneGeneralByCombatPlayerId";
        public static string DeleteDamageTaken = "DeleteDamageTakenByCombatPlayerId";
        public static string DeleteDamageTakenGeneral = "DeleteDamageTakenGeneralByCombatPlayerId";
        public static string DeleteResourceRecovery = "DeleteResourceRecoveryByCombatPlayerId";
        public static string DeleteResourceRecoveryGeneral = "DeleteResourceRecoveryGeneralByCombatPlayerId";

        public static async Task CreateProceduresAsync(DbContext dbContext)
        {
            await SelectStoredProcedures(dbContext);
            await InsertIntoStoredProcedures(dbContext);
            await DeleteStoredProcedures(dbContext);
        }

        private static async Task SelectStoredProcedures(DbContext dbContext)
        {
            var query = @"CREATE PROCEDURE GetCombatByCombatLogId (@combatLogId INT)
                          AS SELECT *
                          FROM Combat
                          WHERE CombatLogId = @combatLogId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetCombatPlayerByCombatId (@combatId INT)
                          AS SELECT *
                          FROM CombatPlayer
                          WHERE CombatId = @combatId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetDamageDoneByCombatPlayerId (@CombatPlayerId INT)
                          AS SELECT *
                          FROM DamageDone
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetDamageDoneGeneralByCombatPlayerId (@CombatPlayerId INT)
                          AS SELECT *
                          FROM DamageDoneGeneral
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetHealDoneByCombatPlayerId (@CombatPlayerId INT)
                          AS SELECT *
                          FROM HealDone
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetHealDoneGeneralByCombatPlayerId (@CombatPlayerId INT)
                          AS SELECT *
                          FROM HealDoneGeneral
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetDamageTakenByCombatPlayerId (@CombatPlayerId INT)
                          AS SELECT *
                          FROM DamageTaken
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetDamageTakenGeneralByCombatPlayerId (@CombatPlayerId INT)
                          AS SELECT *
                          FROM DamageTakenGeneral
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetResourceRecoveryByCombatPlayerId (@CombatPlayerId INT)
                          AS SELECT *
                          FROM ResourceRecovery
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetResourceRecoveryGeneralByCombatPlayerId (@CombatPlayerId INT)
                          AS SELECT *
                          FROM ResourceRecoveryGeneral
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);
        }

        private static async Task InsertIntoStoredProcedures(DbContext dbContext)
        {
            var query = @"CREATE PROCEDURE InsertIntoCombat (@DungeonName NVARCHAR (MAX), @Name NVARCHAR (MAX), @DamageDone INT, @HealDone INT, @DamageTaken INT,
                                           @EnergyRecovery INT, @DeathNumber INT, @UsedBuffs INT, @IsWin BIT, @StartDate DATETIMEOFFSET (7), @FinishDate DATETIMEOFFSET (7), @CombatLogId INT)
                          AS INSERT INTO Combat
                          VALUES (@DungeonName, @Name, @DamageDone, @HealDone, @DamageTaken, @EnergyRecovery, @DeathNumber, @UsedBuffs, @IsWin, @StartDate, @FinishDate, @CombatLogId)";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE InsertIntoCombatPlayer (@UserName NVARCHAR (MAX), @DamageDone INT, @HealDone INT, @DamageTaken INT, @EnergyRecovery INT, @UsedBuffs INT, @CombatId INT)
                          AS INSERT INTO CombatPlayer
                          VALUES (@UserName, @DamageDone, @HealDone, @DamageTaken, @EnergyRecovery, @UsedBuffs, @CombatId)";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE InsertIntoDamageDone (@Value INT, @Time NVARCHAR (MAX), @FromPlayer NVARCHAR (MAX), @ToEnemy NVARCHAR (MAX), @SpellOrItem NVARCHAR (MAX),
                                           @IsDodge BIT, @IsParry BIT, @IsMiss BIT, @IsResist BIT, @IsImmune BIT, @IsCrit BIT, @CombatPlayerId INT)
                          AS INSERT INTO DamageDone
                          VALUES (@Value, @Time, @FromPlayer, @ToEnemy, @SpellOrItem, @IsDodge, @IsParry, @IsMiss, @IsResist, @IsImmune, @IsCrit, @CombatPlayerId)";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE InsertIntoDamageDoneGeneral (@Value INT, @DamagePerSecond FLOAT (53), @SpellOrItem NVARCHAR (MAX), @CritNumber INT,
                                       @MissNumber INT, @CastNumber INT, @MinValue INT, @MaxValue INT, @AverageValue FLOAT (53), @CombatPlayerId INT)
                          AS INSERT INTO DamageDoneGeneral
                          VALUES (@Value, @DamagePerSecond, @SpellOrItem, @CritNumber, @MissNumber, @CastNumber, @MinValue, @MaxValue, @AverageValue, @CombatPlayerId)";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE InsertIntoHealDone (@ValueWithOverheal INT, @Time NVARCHAR (MAX), @Overheal INT, @Value INT,
                                       @FromPlayer NVARCHAR (MAX), @ToPlayer NVARCHAR (MAX), @SpellOrItem NVARCHAR (MAX), @CurrentHealth INT, @MaxHealth INT, @IsCrit BIT, @IsFullOverheal BIT, @CombatPlayerId INT)
                          AS INSERT INTO HealDone
                          VALUES (@ValueWithOverheal, @Time, @Overheal, @Value, @FromPlayer, @ToPlayer, @SpellOrItem, @CurrentHealth, @MaxHealth, @IsCrit, @IsFullOverheal, @CombatPlayerId)";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE InsertIntoHealDoneGeneral (@Value INT, @HealPerSecond FLOAT (53), @SpellOrItem NVARCHAR (MAX),
                                           @CritNumber INT, @CastNumber INT, @MinValue INT, @MaxValue INT, @AverageValue FLOAT (53), @CombatPlayerId INT)
                          AS INSERT INTO HealDoneGeneral
                          VALUES (@Value, @HealPerSecond, @SpellOrItem, @CritNumber, @CastNumber, @MinValue, @MaxValue, @AverageValue, @CombatPlayerId)";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE InsertIntoDamageTaken (@Value INT, @Time NVARCHAR (MAX), @From NVARCHAR (MAX),
                                           @To NVARCHAR (MAX), @SpellOrItem NVARCHAR (MAX), @IsDodge BIT, @IsParry BIT, @IsMiss BIT, @IsResist BIT, @IsImmune BIT, @IsCrushing BIT, @CombatPlayerId INT)
                          AS INSERT INTO DamageTaken
                          VALUES (@Value, @Time, @From, @To, @SpellOrItem, @IsDodge, @IsParry, @IsMiss, @IsResist, @IsImmune, @IsCrushing, @CombatPlayerId)";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE InsertIntoDamageTakenGeneral (@Value INT, @DamageTakenPerSecond FLOAT (53), @SpellOrItem NVARCHAR (MAX),
                                           @CritNumber INT, @MissNumber INT, @CastNumber INT, @MinValue INT, @MaxValue INT, @AverageValue FLOAT (53), @CombatPlayerId INT)
                          AS INSERT INTO DamageTakenGeneral
                          VALUES (@Value, @DamageTakenPerSecond, @SpellOrItem, @CritNumber, @MissNumber, @CastNumber, @MinValue, @MaxValue, @AverageValue, @CombatPlayerId)";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE InsertIntoResourceRecovery (@Value FLOAT (53), @Time NVARCHAR (MAX), @SpellOrItem NVARCHAR (MAX), @CombatPlayerId INT)
                          AS INSERT INTO ResourceRecovery
                          VALUES (@Value, @Time, @SpellOrItem, @CombatPlayerId)";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE InsertIntoResourceRecoveryGeneral (@Value INT, @ResourcePerSecond FLOAT (53), @SpellOrItem NVARCHAR (MAX), @CastNumber INT, @MinValue INT, @MaxValue INT, @AverageValue FLOAT (53), @CombatPlayerId INT)
                          AS INSERT INTO ResourceRecoveryGeneral
                          VALUES (@Value, @ResourcePerSecond, @SpellOrItem, @CastNumber, @MinValue, @MaxValue, @AverageValue, @CombatPlayerId)";
            await dbContext.Database.ExecuteSqlRawAsync(query);
        }

        private static async Task DeleteStoredProcedures(DbContext dbContext)
        {
            var query = @"CREATE PROCEDURE DeleteCombatByCombatLogId (@combatLogId INT)
                          AS DELETE FROM Combat 
                          WHERE CombatLogId = @combatLogId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteCombatPlayerByCombatId (@combatId INT)
                          AS DELETE FROM CombatPlayer 
                          WHERE CombatId = @combatId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteHealDoneByCombatPlayerId (@CombatPlayerId INT)
                          AS DELETE FROM HealDone 
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteHealDoneGeneralByCombatPlayerId (@CombatPlayerId INT)
                          AS DELETE FROM HealDoneGeneral
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteDamageDoneByCombatPlayerId (@CombatPlayerId INT)
                          AS DELETE FROM DamageDone 
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteDamageDoneGeneralByCombatPlayerId (@CombatPlayerId INT)
                          AS DELETE FROM DamageDoneGeneral 
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteDamageTakenByCombatPlayerId (@CombatPlayerId INT)
                          AS DELETE FROM DamageTaken 
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteDamageTakenGeneralByCombatPlayerId (@CombatPlayerId INT)
                          AS DELETE FROM DamageTakenGeneral 
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteResourceRecoveryByCombatPlayerId (@CombatPlayerId INT)
                          AS DELETE FROM ResourceRecovery 
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteResourceRecoveryGeneralByCombatPlayerId (@CombatPlayerId INT)
                          AS DELETE FROM ResourceRecoveryGeneral 
                          WHERE CombatPlayerId = @CombatPlayerId";
            await dbContext.Database.ExecuteSqlRawAsync(query);
        }
    }
}
