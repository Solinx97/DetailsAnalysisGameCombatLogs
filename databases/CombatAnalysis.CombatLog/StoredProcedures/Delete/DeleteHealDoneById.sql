CREATE PROCEDURE DeleteHealDoneById (@id INT)
	AS DELETE FROM HealDone
	WHERE Id = @id