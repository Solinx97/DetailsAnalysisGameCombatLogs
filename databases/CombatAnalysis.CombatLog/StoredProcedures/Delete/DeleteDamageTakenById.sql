CREATE PROCEDURE DeleteDamageTakenById (@id INT)
	AS DELETE FROM DamageTaken
	WHERE Id = @id