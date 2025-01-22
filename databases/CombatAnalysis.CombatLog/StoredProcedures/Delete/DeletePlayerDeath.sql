CREATE PROCEDURE DeletePlayerDeathById (@id INT)
	AS DELETE FROM PlayerDeath
	WHERE Id = @id
