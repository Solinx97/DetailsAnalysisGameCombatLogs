CREATE PROCEDURE DeletePlayerParseInfoById (@id INT)
	AS DELETE FROM PlayerParseInfo
	WHERE Id = @id