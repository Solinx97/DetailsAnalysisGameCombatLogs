CREATE PROCEDURE GetHealDoneById (@id INT)
	AS SELECT * 
	FROM HealDone
	WHERE Id = @id