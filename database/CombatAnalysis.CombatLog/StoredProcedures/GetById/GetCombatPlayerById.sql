CREATE PROCEDURE GetCombatPlayerById (@id INT)
	AS SELECT * 
	FROM CombatPlayer
	WHERE Id = @id