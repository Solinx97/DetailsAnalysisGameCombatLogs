CREATE PROCEDURE UpdateCombatLog (@Id INT,@Name NVARCHAR (MAX),@Date DATETIMEOFFSET (7),@NumberReadyCombats INT,@CombatsInQueue INT, @IsReady BIT)
	AS UPDATE CombatLog
	SET Name = @Name,Date = @Date,NumberReadyCombats = @NumberReadyCombats,CombatsInQueue = @CombatsInQueue,IsReady = @IsReady
	WHERE Id = @Id