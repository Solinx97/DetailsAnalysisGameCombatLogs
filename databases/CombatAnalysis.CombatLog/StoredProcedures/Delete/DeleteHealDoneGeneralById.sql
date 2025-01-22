CREATE PROCEDURE DeleteHealDoneGeneralById (@id INT)
	AS DELETE FROM HealDoneGeneral
	WHERE Id = @id