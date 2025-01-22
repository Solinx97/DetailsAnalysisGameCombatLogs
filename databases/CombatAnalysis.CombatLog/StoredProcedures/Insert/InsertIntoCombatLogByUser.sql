CREATE PROCEDURE InsertIntoCombatLogByUser (@Id INT,@PersonalLogType INT,@NumberReadyCombats INT,@CombatsInQueue INT, @IsReady BIT,@CombatLogId INT, @AppUserId NVARCHAR (MAX))
	AS
	DECLARE @OutputTbl TABLE (Id INT,PersonalLogType INT,NumberReadyCombats INT,CombatsInQueue INT,IsReady BIT,CombatLogId INT,AppUserId NVARCHAR (MAX))
	INSERT INTO CombatLogByUser
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@PersonalLogType,@NumberReadyCombats,@CombatsInQueue,@IsReady,@CombatLogId,@AppUserId)
	SELECT * FROM @OutputTbl