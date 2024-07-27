CREATE PROCEDURE UpdateCombatLogByUser (@Id INT,@PersonalLogType INT,@NumberReadyCombats INT,@CombatsInQueue INT, @IsReady BIT,@CombatLogId INT, @AppUserId NVARCHAR (MAX))
	AS UPDATE CombatLogByUser
	SET PersonalLogType = @PersonalLogType,NumberReadyCombats = @NumberReadyCombats,CombatsInQueue = @CombatsInQueue,IsReady = @IsReady,CombatLogId = @CombatLogId,AppUserId = @AppUserId
	WHERE Id = @Id