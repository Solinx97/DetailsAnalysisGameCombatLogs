CREATE PROCEDURE [dbo].[InsertIntoCombat]
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
	DECLARE @OutputTbl TABLE (Id INT,DungeonName NVARCHAR (MAX),Name NVARCHAR (MAX),DamageDone INT,HealDone INT,DamageTaken INT,EnergyRecovery INT,DeathNumber INT,UsedBuffs INT,IsWin BIT,StartDate DATETIMEOFFSET (7),FinishDate DATETIMEOFFSET (7),CombatLogId INT)
	INSERT INTO Combat
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@DungeonName,@Name,@DamageDone,@HealDone,@DamageTaken,@EnergyRecovery,@DeathNumber,@UsedBuffs,@IsWin,@StartDate,@FinishDate,@CombatLogId)
	SELECT * FROM @OutputTbl
RETURN 0
