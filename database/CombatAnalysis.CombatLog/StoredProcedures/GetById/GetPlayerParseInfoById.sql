CREATE PROCEDURE GetPlayerParseInfoById (@id INT)
	AS SELECT * 
	FROM PlayerParseInfo
	WHERE Id = @id