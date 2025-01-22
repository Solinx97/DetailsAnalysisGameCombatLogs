CREATE PROCEDURE InsertIntoCombatLog (@Name NVARCHAR (MAX),@Date DATETIMEOFFSET (7),@NumberReadyCombats INT,@CombatsInQueue INT, @IsReady BIT)
	AS
	DECLARE @OutputTbl TABLE (Id INT,Name NVARCHAR (MAX),Date DATETIMEOFFSET (7),NumberReadyCombats INT, CombatsInQueue INT, IsReady BIT)
	INSERT INTO CombatLog
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Name,@Date,@IsReady)
	SELECT * FROM @OutputTbl