CREATE PROCEDURE [dbo].[UpdateCombat]
	@Id INT,
	@DungeonName NVARCHAR (MAX),
	@Name NVARCHAR (MAX),
	@DamageDone INT,
	@HealDone INT,
	@DamageTaken INT,
	@EnergyRecovery INT,
	@DeathNumber INT,
	@UsedBuffs INT,
	@IsWin BIT,
	@StartDate DATETIMEOFFSET (7),
	@FinishDate DATETIMEOFFSET (7),
	@CombatLogId INT
AS
	UPDATE Combat
	SET DungeonName = @DungeonName,Name = @Name,DamageDone = @DamageDone,HealDone = @HealDone,DamageTaken = @DamageTaken,EnergyRecovery = @EnergyRecovery,DeathNumber = @DeathNumber,UsedBuffs = @UsedBuffs,IsWin = @IsWin,StartDate = @StartDate,FinishDate = @FinishDate,CombatLogId = @CombatLogId
	WHERE Id = @Id
RETURN 0
