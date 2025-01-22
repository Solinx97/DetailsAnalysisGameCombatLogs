CREATE PROCEDURE GetPlayerDeathById (@id INT)
	AS SELECT * 
	FROM PlayerDeath
	WHERE Id = @id