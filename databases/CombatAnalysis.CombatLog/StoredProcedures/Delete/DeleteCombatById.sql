CREATE PROCEDURE DeleteCombatById (@id INT)
	AS DELETE FROM Combat
	WHERE Id = @id