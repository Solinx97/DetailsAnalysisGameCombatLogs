CREATE PROCEDURE GetCombatLogByUserById (@id INT)
	AS SELECT * 
	FROM CombatLogByUser
	WHERE Id = @id