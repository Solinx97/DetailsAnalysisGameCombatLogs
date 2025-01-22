CREATE PROCEDURE InsertIntoCombat (@LocallyNumber INT,@DungeonName NVARCHAR (MAX),@Name NVARCHAR (MAX),@Difficulty INT,@DamageDone INT,@HealDone INT,@DamageTaken INT,@EnergyRecovery INT,@DeathNumber INT,@IsWin BIT,@StartDate DATETIMEOFFSET (7),@FinishDate DATETIMEOFFSET (7),@IsReady BIT,@CombatLogId INT)
	AS
	DECLARE @OutputTbl TABLE (Id INT,LocallyNumber INT,DungeonName NVARCHAR (MAX),Name NVARCHAR (MAX),Difficulty INT,DamageDone INT,HealDone INT,DamageTaken INT,EnergyRecovery INT,DeathNumber INT,IsWin BIT,StartDate DATETIMEOFFSET (7),FinishDate DATETIMEOFFSET (7),IsReady BIT,CombatLogId INT)
	INSERT INTO Combat
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@LocallyNumber,@DungeonName,@Name,@Difficulty,@DamageDone,@HealDone,@DamageTaken,@EnergyRecovery,@DeathNumber,@IsWin,@StartDate,@FinishDate,@IsReady,@CombatLogId)
	SELECT * FROM @OutputTbl