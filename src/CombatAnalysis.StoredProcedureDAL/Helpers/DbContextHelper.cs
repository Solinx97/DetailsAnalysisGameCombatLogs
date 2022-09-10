using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CombatAnalysis.StoredProcedureDAL.Helpers
{
    public static class DbProcedureHelper
    {
        public static async Task CreateProceduresAsync(DbContext dbContext)
        {
            await SelectAllStoredProcedures(dbContext);
            await SelectStoredProcedures(dbContext);
            await InsertIntoStoredProcedures(dbContext);
            await UpdateStoredProcedures(dbContext);
            await DeleteStoredProcedures(dbContext);
        }

        private static async Task SelectAllStoredProcedures(DbContext dbContext)
        {
            var query = @"CREATE PROCEDURE GetAllUsers
                          AS SELECT *
                          FROM User";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetAllCombatLog
                          AS SELECT *
                          FROM CombatLog";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetAllCombatLogByUser
                          AS SELECT *
                          FROM CombatLogByUser";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetAllCombat
                          AS SELECT *
                          FROM Combat";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetAllCombatPlayer
                          AS SELECT *
                          FROM CombatPlayer";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetAllDamageDone
                          AS SELECT *
                          FROM DamageDone";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetAllDamageDoneGeneral
                          AS SELECT *
                          FROM DamageDoneGeneral";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetAllHealDone
                          AS SELECT *
                          FROM HealDone";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetAllHealDoneGeneral
                          AS SELECT *
                          FROM HealDoneGeneral";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetAllDamageTaken
                          AS SELECT *
                          FROM DamageTaken";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetAllDamageTakenGeneral
                          AS SELECT *
                          FROM DamageTakenGeneral";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetAllResourceRecovery
                          AS SELECT *
                          FROM ResourceRecovery";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetAllResourceRecoveryGeneral
                          AS SELECT *
                          FROM ResourceRecoveryGeneral";
            await dbContext.Database.ExecuteSqlRawAsync(query);
        }

        private static async Task SelectStoredProcedures(DbContext dbContext)
        {
            var query = @"CREATE PROCEDURE GetUserById (@id NVARCHAR (MAX))
                          AS SELECT *
                          FROM User
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetCombatLogById (@id INT)
                          AS SELECT *
                          FROM CombatLog
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetCombatLogByUserById (@id INT)
                          AS SELECT *
                          FROM CombatLogByUser
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetCombatById (@id INT)
                          AS SELECT *
                          FROM Combat
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetCombatPlayerById (@id INT)
                          AS SELECT *
                          FROM CombatPlayer
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetDamageDoneById (@id INT)
                          AS SELECT *
                          FROM DamageDone
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetDamageDoneGeneralById (@id INT)
                          AS SELECT *
                          FROM DamageDoneGeneral
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetHealDoneById (@id INT)
                          AS SELECT *
                          FROM HealDone
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetHealDoneGeneralById (@id INT)
                          AS SELECT *
                          FROM HealDoneGeneral
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetDamageTakenById (@id INT)
                          AS SELECT *
                          FROM DamageTaken
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetDamageTakenGeneralById (@id INT)
                          AS SELECT *
                          FROM DamageTakenGeneral
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetResourceRecoveryById (@id INT)
                          AS SELECT *
                          FROM ResourceRecovery
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE GetResourceRecoveryGeneralById (@id INT)
                          AS SELECT *
                          FROM ResourceRecoveryGeneral
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);
        }

        private static async Task InsertIntoStoredProcedures(DbContext dbContext)
        {
            var query = @"CREATE PROCEDURE InsertIntoUser (@Email NVARCHAR (MAX), @Password NVARCHAR (MAX))
                          AS INSERT INTO User
                          VALUES (@Email, @Password)";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE InsertIntoCombatLog (@Name NVARCHAR (MAX), @Date DATETIMEOFFSET (7), @IsReady BIT)
                          AS INSERT INTO CombatLog
                          VALUES (@Name, @Date, @IsReady)";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE InsertIntoCombatLogByUser (@UserId NVARCHAR (MAX), @PersonalLogType INT, @CombatLogId INT)
                          AS INSERT INTO CombatLogByUser
                          VALUES (@UserId, @Name, @PersonalLogType, @CombatLogId)";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE InsertIntoCombat (@DungeonName NVARCHAR (MAX), @Name NVARCHAR (MAX), @DamageDone INT, @HealDone INT, @DamageTaken INT,
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

        private static async Task UpdateStoredProcedures(DbContext dbContext)
        {
            var query = @"CREATE PROCEDURE UpdateUser (@Id NVARCHAR (MAX), @Email NVARCHAR (MAX), @Password NVARCHAR (MAX))
                          AS UPDATE User
                          SET (@Email, @Password)
                          WHERE Id = @Id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE UpdateCombatLog (@Id INT, @Name NVARCHAR (MAX), @Date DATETIMEOFFSET (7), @IsReady BIT)
                          AS UPDATE CombatLog
                          SET (@Name, @Date, @IsReady)
                          WHERE Id = @Id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE UpdateCombatLogByUser (@Id INT, @UserId NVARCHAR (MAX), @PersonalLogType INT, @CombatLogId INT)
                          AS UPDATE CombatLogByUser
                          SET (@UserId, @PersonalLogType, @CombatLogId)
                          WHERE Id = @Id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE UpdateCombat (@Id INT, @DungeonName NVARCHAR (MAX), @Name NVARCHAR (MAX), @DamageDone INT, @HealDone INT, @DamageTaken INT,
                                           @EnergyRecovery INT, @DeathNumber INT, @UsedBuffs INT, @IsWin BIT, @StartDate DATETIMEOFFSET (7), @FinishDate DATETIMEOFFSET (7), @CombatLogId INT)
                          AS UPDATE Combat
                          SET (@DungeonName, @Name, @DamageDone, @HealDone, @DamageTaken, @EnergyRecovery, @DeathNumber, @UsedBuffs, @IsWin, @StartDate, @FinishDate, @CombatLogId)
                          WHERE Id = @Id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE UpdateCombatPlayer (@Id INT, @UserName NVARCHAR (MAX), @DamageDone INT, @HealDone INT, @DamageTaken INT, @EnergyRecovery INT, @UsedBuffs INT, @CombatId INT)
                          AS UPDATE CombatPlayer
                          SET (@UserName, @DamageDone, @HealDone, @DamageTaken, @EnergyRecovery, @UsedBuffs, @CombatId)
                          WHERE Id = @Id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE UpdateDamageDone (@Id INT, @Value INT, @Time NVARCHAR (MAX), @FromPlayer NVARCHAR (MAX), @ToEnemy NVARCHAR (MAX), @SpellOrItem NVARCHAR (MAX),
                                           @IsDodge BIT, @IsParry BIT, @IsMiss BIT, @IsResist BIT, @IsImmune BIT, @IsCrit BIT, @CombatPlayerId INT)
                          AS UPDATE DamageDone
                          SET (@Value, @Time, @FromPlayer, @ToEnemy, @SpellOrItem, @IsDodge, @IsParry, @IsMiss, @IsResist, @IsImmune, @IsCrit, @CombatPlayerId)
                          WHERE Id = @Id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE UpdateDamageDoneGeneral (@Id INT, @Value INT, @DamagePerSecond FLOAT (53), @SpellOrItem NVARCHAR (MAX), @CritNumber INT,
                                       @MissNumber INT, @CastNumber INT, @MinValue INT, @MaxValue INT, @AverageValue FLOAT (53), @CombatPlayerId INT)
                          AS UPDATE DamageDoneGeneral
                          SET (@Value, @DamagePerSecond, @SpellOrItem, @CritNumber, @MissNumber, @CastNumber, @MinValue, @MaxValue, @AverageValue, @CombatPlayerId)
                          WHERE Id = @Id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE UpdateHealDone (@Id INT, @ValueWithOverheal INT, @Time NVARCHAR (MAX), @Overheal INT, @Value INT,
                                       @FromPlayer NVARCHAR (MAX), @ToPlayer NVARCHAR (MAX), @SpellOrItem NVARCHAR (MAX), @CurrentHealth INT, @MaxHealth INT, @IsCrit BIT, @IsFullOverheal BIT, @CombatPlayerId INT)
                          AS UPDATE HealDone
                          SET (@ValueWithOverheal, @Time, @Overheal, @Value, @FromPlayer, @ToPlayer, @SpellOrItem, @CurrentHealth, @MaxHealth, @IsCrit, @IsFullOverheal, @CombatPlayerId)
                          WHERE Id = @Id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE UpdateHealDoneGeneral (@Id INT, @Value INT, @HealPerSecond FLOAT (53), @SpellOrItem NVARCHAR (MAX),
                                           @CritNumber INT, @CastNumber INT, @MinValue INT, @MaxValue INT, @AverageValue FLOAT (53), @CombatPlayerId INT)
                          AS UPDATE HealDoneGeneral
                          SET (@Value, @HealPerSecond, @SpellOrItem, @CritNumber, @CastNumber, @MinValue, @MaxValue, @AverageValue, @CombatPlayerId)
                          WHERE Id = @Id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE UpdateDamageTaken (@Id INT, @Value INT, @Time NVARCHAR (MAX), @From NVARCHAR (MAX),
                                           @To NVARCHAR (MAX), @SpellOrItem NVARCHAR (MAX), @IsDodge BIT, @IsParry BIT, @IsMiss BIT, @IsResist BIT, @IsImmune BIT, @IsCrushing BIT, @CombatPlayerId INT)
                          AS UPDATE DamageTaken
                          SET (@Value, @Time, @From, @To, @SpellOrItem, @IsDodge, @IsParry, @IsMiss, @IsResist, @IsImmune, @IsCrushing, @CombatPlayerId)
                          WHERE Id = @Id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE UpdateDamageTakenGeneral (@Id INT, @Value INT, @DamageTakenPerSecond FLOAT (53), @SpellOrItem NVARCHAR (MAX),
                                           @CritNumber INT, @MissNumber INT, @CastNumber INT, @MinValue INT, @MaxValue INT, @AverageValue FLOAT (53), @CombatPlayerId INT)
                          AS UPDATE DamageTakenGeneral
                          SET (@Value, @DamageTakenPerSecond, @SpellOrItem, @CritNumber, @MissNumber, @CastNumber, @MinValue, @MaxValue, @AverageValue, @CombatPlayerId)
                          WHERE Id = @Id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE UpdateResourceRecovery (@Id INT, @Value FLOAT (53), @Time NVARCHAR (MAX), @SpellOrItem NVARCHAR (MAX), @CombatPlayerId INT)
                          AS UPDATE ResourceRecovery
                          SET (@Value, @Time, @SpellOrItem, @CombatPlayerId)
                          WHERE Id = @Id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE UpdateResourceRecoveryGeneral (@Id INT, @Value INT, @ResourcePerSecond FLOAT (53), @SpellOrItem NVARCHAR (MAX), @CastNumber INT, @MinValue INT, @MaxValue INT, @AverageValue FLOAT (53), @CombatPlayerId INT)
                          AS UPDATE ResourceRecoveryGeneral
                          SET (@Value, @ResourcePerSecond, @SpellOrItem, @CastNumber, @MinValue, @MaxValue, @AverageValue, @CombatPlayerId)
                          WHERE Id = @Id";
            await dbContext.Database.ExecuteSqlRawAsync(query);
        }

        private static async Task DeleteStoredProcedures(DbContext dbContext)
        {
            var query = @"CREATE PROCEDURE DeleteUserById (@id NVARCHAR (MAX))
                          AS DELETE FROM User 
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteCombatLogById (@id INT)
                          AS DELETE FROM CombatLog 
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteCombatLogByUserById (@id INT)
                          AS DELETE FROM CombatLogByUser
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteCombatById (@id INT)
                          AS DELETE FROM Combat 
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteCombatPlayerById (@id INT)
                          AS DELETE FROM CombatPlayer 
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteHealDoneById (@id INT)
                          AS DELETE FROM HealDone 
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteHealDoneGeneralById (@id INT)
                          AS DELETE FROM HealDoneGeneral
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteDamageDoneById (@id INT)
                          AS DELETE FROM DamageDone 
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteDamageDoneGeneralById (@id INT)
                          AS DELETE FROM DamageDoneGeneral 
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteDamageTakenById (@id INT)
                          AS DELETE FROM DamageTaken 
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteDamageTakenGeneralById (@id INT)
                          AS DELETE FROM DamageTakenGeneral 
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteResourceRecoveryById (@id INT)
                          AS DELETE FROM ResourceRecovery 
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);

            query = @"CREATE PROCEDURE DeleteResourceRecoveryGeneralById (@id INT)
                          AS DELETE FROM ResourceRecoveryGeneral 
                          WHERE Id = @id";
            await dbContext.Database.ExecuteSqlRawAsync(query);
        }
    }
}
