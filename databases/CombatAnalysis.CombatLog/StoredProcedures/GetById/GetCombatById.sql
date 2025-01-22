CREATE PROCEDURE GetCombatById (@id INT)
	AS SELECT * 
	FROM Combat
	WHERE Id = @id