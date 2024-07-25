CREATE PROCEDURE DeleteCombatLogByUserById (@id INT)
	AS DELETE FROM CombatLogByUser
	WHERE Id = @id