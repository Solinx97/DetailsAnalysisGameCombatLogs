CREATE PROCEDURE GetCombatLogById (@id INT)
	AS SELECT * 
	FROM CombatLog
	WHERE Id = @id