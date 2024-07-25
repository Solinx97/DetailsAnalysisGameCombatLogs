CREATE PROCEDURE GetHealDoneGeneralById (@id INT)
	AS SELECT * 
	FROM HealDoneGeneral
	WHERE Id = @id