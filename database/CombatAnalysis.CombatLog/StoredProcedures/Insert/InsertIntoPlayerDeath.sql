CREATE PROCEDURE InsertIntoPlayerDeath (@Date DATETIMEOFFSET (7),@CombatPlayerId INT)
	AS
	DECLARE @OutputTbl TABLE (Id INT,Date DATETIMEOFFSET (7),CombatPlayerId INT)
	INSERT INTO PlayerDeath
	OUTPUT INSERTED.* INTO @OutputTbl
	VALUES (@Date,@CombatPlayerId)
	SELECT * FROM @OutputTbl
