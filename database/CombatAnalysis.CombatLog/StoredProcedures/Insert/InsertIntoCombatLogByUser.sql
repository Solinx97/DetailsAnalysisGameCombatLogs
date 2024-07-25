CREATE PROCEDURE InsertIntoCombatLogByUser (@CombatLogId INT,@UserId NVARCHAR (MAX),@PersonalLogType INT)
	AS
	DECLARE @OutputTbl TABLE (Id INT,CombatLogId INT,UserId NVARCHAR (MAX),PersonalLogType INT)
	INSERT INTO CombatLogByUser
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@CombatLogId,@UserId,@PersonalLogType)
	SELECT * FROM @OutputTbl