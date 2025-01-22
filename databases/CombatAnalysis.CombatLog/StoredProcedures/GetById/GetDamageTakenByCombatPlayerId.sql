CREATE PROCEDURE GetDamageTakenByCombatPlayerId (@combatPlayerId INT)
	AS SELECT * 
	FROM DamageTaken
	WHERE CombatPlayerId = @combatPlayerId