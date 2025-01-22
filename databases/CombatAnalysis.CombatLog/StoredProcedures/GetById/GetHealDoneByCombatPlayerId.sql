CREATE PROCEDURE GetHealDoneByCombatPlayerId (@combatPlayerId INT)
	AS SELECT * 
	FROM HealDone
	WHERE CombatPlayerId = @combatPlayerId