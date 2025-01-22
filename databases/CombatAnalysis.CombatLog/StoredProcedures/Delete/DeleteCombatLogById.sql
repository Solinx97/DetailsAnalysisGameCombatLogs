CREATE PROCEDURE DeleteCombatLogById (@id INT)
	AS DELETE FROM CombatLog
	WHERE Id = @id