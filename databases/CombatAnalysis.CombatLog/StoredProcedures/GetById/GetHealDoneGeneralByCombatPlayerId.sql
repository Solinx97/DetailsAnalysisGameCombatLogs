CREATE PROCEDURE GetHealDoneGeneralByCombatPlayerId (@combatPlayerId INT)
	AS SELECT * 
	FROM HealDoneGeneral
	WHERE CombatPlayerId = @combatPlayerId