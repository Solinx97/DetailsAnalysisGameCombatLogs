CREATE PROCEDURE GetResourceRecoveryByCombatPlayerId (@combatPlayerId INT)
	AS SELECT * 
	FROM ResourceRecovery
	WHERE CombatPlayerId = @combatPlayerId