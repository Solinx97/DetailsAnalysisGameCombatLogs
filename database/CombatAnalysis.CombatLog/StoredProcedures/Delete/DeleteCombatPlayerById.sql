CREATE PROCEDURE DeleteCombatPlayerById (@id INT)
	AS DELETE FROM CombatPlayer
	WHERE Id = @id