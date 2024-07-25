CREATE PROCEDURE GetDamageTakenById (@id INT)
	AS SELECT * 
	FROM DamageTaken
	WHERE Id = @id